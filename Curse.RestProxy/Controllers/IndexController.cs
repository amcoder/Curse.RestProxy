using System.Web.Http;

namespace Curse.RestProxy.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api")]
    public class IndexController : ApiController
    {
        [HttpGet]
        [Route("", Name = "Index")]
        public IHttpActionResult Index()
        {
            return Ok(new { Website = "http://github.com/amiller/Curse.RestProxy" });
        }
    }
}
