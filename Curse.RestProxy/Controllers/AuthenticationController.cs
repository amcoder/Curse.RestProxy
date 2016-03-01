using System.Threading.Tasks;
using System.Web.Http;
using Curse.RestProxy.Models;

namespace Curse.RestProxy.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api")]
    public class AuthenticationController : ApiController
    {
        private LoginService.IClientLoginService ClientLoginService { get; set; }

        public AuthenticationController(LoginService.IClientLoginService clientLoginService)
        {
            ClientLoginService = clientLoginService;
        }

        [HttpPost]
        [Route("authenticate", Name = "Authenticate")]
        public async Task<IHttpActionResult> Authenticate(AuthenticationRequest request)
        {
            if(request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Username and password are required");

            var result = await ClientLoginService.LoginAsync(new LoginService.LoginRequest()
            {
                Username = request.Username,
                Password = request.Password
            });

            if(result.Status != LoginService.AuthenticationStatus.Success)
                return Unauthorized();

            return Ok(result);
        }
    }
}
