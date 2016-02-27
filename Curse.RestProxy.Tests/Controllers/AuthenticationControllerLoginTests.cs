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
    public class AuthenticationControllerLoginTests
    {
        /*
         * Service Integration
         */
        [TestMethod]
        public void LoginCallsClientLoginService()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.Success
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var response = controller.Login(new Models.LoginRequest()
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
        public void LoginReturnsOkWhenLoginSucceeds()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.Success
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var result = controller.Login(new Models.LoginRequest()
            {
                UserName = "username",
                Password = "password"
            }).Result;

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<LoginService.LoginResponse>),
                "Login should return ok when login succeeds");
        }

        [TestMethod]
        public void LoginBodyContainsClientLoginResponseWhenLoginSucceeds()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.Success
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var result = controller.Login(new Models.LoginRequest()
            {
                UserName = "username",
                Password = "password"
            }).Result as OkNegotiatedContentResult<LoginService.LoginResponse>;

            Assert.AreEqual(clientLoginResponse, result.Content,
                "Login should return the client login response when login succeeds");
        }

        /*
         * Login fails
         */
        [TestMethod]
        public void LoginReturnsUnauthorizedWhenLoginFails()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.InvalidPassword
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var result = controller.Login(new Models.LoginRequest()
            {
                UserName = "username",
                Password = "password"
            }).Result;

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult),
                "Login should return unauthorized when login fails");
        }

        /*
         * Input Validation
         */
        [TestMethod]
        public void LoginReturnsBadRequestWhenLoginRequestNull()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.InvalidPassword
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var result = controller.Login(null).Result;

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "Login should return bad request when the login request is null");
        }

        [TestMethod]
        public void LoginReturnsBadRequestWhenUserNameNull()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.InvalidPassword
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var result = controller.Login(new Models.LoginRequest()
            {
                UserName = null,
                Password = "password"
            }).Result;

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "Login should return bad request when the username is null");
        }

        [TestMethod]
        public void LoginReturnsBadRequestWhenPasswordNull()
        {
            var clientLoginResponse = new LoginService.LoginResponse()
            {
                Status = LoginService.AuthenticationStatus.InvalidPassword
            };
            var service = MockClientLoginService(clientLoginResponse);
            var controller = new AuthenticationController(service);

            var result = controller.Login(new Models.LoginRequest()
            {
                UserName = "username",
                Password = null
            }).Result;

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "Login should return bad request when the password is null");
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
