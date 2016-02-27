using System.Threading.Tasks;
using System.Web.Http;
using Curse.RestProxy.Models;

namespace Curse.RestProxy.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/auth")]
    public class AuthenticationController : ApiController
    {
        private LoginService.IClientLoginService ClientLoginService { get; set; }

        public AuthenticationController(LoginService.IClientLoginService clientLoginService)
        {
            ClientLoginService = clientLoginService;
        }

        [HttpPost]
        [Route("login", Name = "Login")]
        public async Task<IHttpActionResult> Login(LoginRequest request)
        {
            if(request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Username and password are required");

            var result = await ClientLoginService.LoginAsync(new LoginService.LoginRequest()
            {
                Username = request.UserName,
                Password = request.Password
            });

            if(result.Status != LoginService.AuthenticationStatus.Success)
                return Unauthorized();

            return Ok(result);
        }
    }
}
