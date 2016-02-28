using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Curse.RestProxy.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Curse.RestProxy.Tests.Controllers
{
    [TestClass]
    public class AuthenticationControllerAuthenticateTests
    {
        /*
         * Service Integration
         */
        [TestMethod]
        public void AuthenticateCallsClientLoginService()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.Success
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var response = controller.Authenticate(new Models.AuthenticationRequest()
            {
                UserName = "username",
                Password = "password"
            }).Result;

            Mock.Get(service).Verify(s =>
                s.LoginAsync(It.Is<LoginService.LoginRequest>(lr =>
                    lr.Username == "username" && lr.Password == "password")
                )
            );
        }

        /*
         * Success
         */
        [TestMethod]
        public void AuthenticateReturnsOkWhenAuthenticationSucceeds()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.Success
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var result = controller.Authenticate(new Models.AuthenticationRequest()
            {
                UserName = "username",
                Password = "password"
            }).Result;

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<LoginService.LoginResponse>),
                "Authenticate should return ok when authentication succeeds");
        }

        [TestMethod]
        public void AuthenticateBodyContainsClientLoginResponseWhenAuthenticationSucceeds()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.Success
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var result = controller.Authenticate(new Models.AuthenticationRequest()
            {
                UserName = "username",
                Password = "password"
            }).Result as OkNegotiatedContentResult<LoginService.LoginResponse>;

            Assert.AreEqual(clientLoginResponse, result.Content,
                "Authenticate should return the client login response when authentication succeeds");
        }

        /*
         * Authentication fails
         */
        [TestMethod]
        public void AuthenticateReturnsUnauthorizedWhenAuthenticationFails()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.InvalidPassword
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var result = controller.Authenticate(new Models.AuthenticationRequest()
            {
                UserName = "username",
                Password = "password"
            }).Result;

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult),
                "Authenticate should return unauthorized when authentication fails");
        }

        /*
         * Input Validation
         */
        [TestMethod]
        public void AuthenticateReturnsBadRequestWhenAuthenticationRequestNull()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.InvalidPassword
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var result = controller.Authenticate(null).Result;

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "Authenticate should return bad request when the authentication request is null");
        }

        [TestMethod]
        public void AuthenticateReturnsBadRequestWhenUserNameNull()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.InvalidPassword
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var result = controller.Authenticate(new Models.AuthenticationRequest()
            {
                UserName = null,
                Password = "password"
            }).Result;

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "Authenticate should return bad request when the username is null");
        }

        [TestMethod]
        public void AuthenticateReturnsBadRequestWhenPasswordNull()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.InvalidPassword
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var result = controller.Authenticate(new Models.AuthenticationRequest()
            {
                UserName = "username",
                Password = null
            }).Result;

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "Authenticate should return bad request when the password is null");
        }

        private LoginService.IClientLoginService MockClientLoginService(LoginService.LoginResponse response)
        {
            return Mock.Of<LoginService.IClientLoginService>(l =>
                l.LoginAsync(It.IsAny<LoginService.LoginRequest>()) ==
                        Task.FromResult(response)
            );
        }
    }
}
