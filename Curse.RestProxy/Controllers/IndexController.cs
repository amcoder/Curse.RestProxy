using System.ServiceModel;
using System.Threading.Tasks;
using System.Web.Http;

namespace Curse.RestProxy.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api")]
    public class IndexController : ApiController
    {
        private LoginService.IClientLoginService ClientLoginService { get; set; }
        private AddOnService.IAddOnService AddOnService { get; set; }

        public IndexController(LoginService.IClientLoginService clientLoginService,
                               AddOnService.IAddOnService addOnService)
        {
            ClientLoginService = clientLoginService;
            AddOnService = addOnService;
        }

        [HttpGet]
        [Route("", Name = "Index")]
        public IHttpActionResult Index()
        {
            return Ok(new { Website = "http://github.com/amiller/Curse.RestProxy" });
        }

        [HttpGet]
        [Route("status", Name = "Status")]
        public async Task<IHttpActionResult> Status()
        {
            return Ok(new
            {
                CurseRestProxy = "Ok",
                LoginService = await GetClientLoginServiceStatus(),
                AddOnService = await GetAddOnServiceStatus()
            });
        }

        private async Task<string> GetClientLoginServiceStatus()
        {
            try
            {
                var result = await ClientLoginService.CheckUsernameAvailabilityAsync(null);
                return "Ok";
            }
            catch(EndpointNotFoundException)
            {
                return "Down";
            }
            catch(FaultException)
            {
                return "Error";
            }
        }

        private async Task<string> GetAddOnServiceStatus()
        {
            try
            {
                var result = await AddOnService.HealthCheckAsync();

                if(result == "Success")
                    return "Ok";

                return result;
            }
            catch(EndpointNotFoundException)
            {
                return "Down";
            }
            catch(FaultException)
            {
                return "Error";
            }
        }

    }
}
