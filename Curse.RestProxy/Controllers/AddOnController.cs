using System.Threading.Tasks;
using System.Web.Http;

namespace Curse.RestProxy.Controllers
{
    [RoutePrefix("api/addon")]
    public class AddOnController : ApiController
    {
        private AddOnService.IAddOnService AddOnService { get; set; }

        public AddOnController(AddOnService.IAddOnService addOnService)
        {
            AddOnService = addOnService;
        }

        [HttpGet]
        [Route("{id}", Name = "GetAddOn")]
        public async Task<IHttpActionResult> GetAddOn(int id)
        {
            var result = await AddOnService.GetAddOnAsync(id);

            if(result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}/description", Name = "GetAddOnDescription")]
        public async Task<IHttpActionResult> GetAddOnDescription(int id)
        {
            var result = await AddOnService.v2GetAddOnDescriptionAsync(id);

            if(result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
