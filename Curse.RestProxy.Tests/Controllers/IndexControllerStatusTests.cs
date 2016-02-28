using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Curse.RestProxy.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Curse.RestProxy.Tests.Controllers
{
    [TestClass]
    public class IndexControllerStatusTests
    {
        [TestMethod]
        public void StatusReturnsOk()
        {
            var controller = new IndexController(Mock.Of<LoginService.IClientLoginService>(),
                                                 Mock.Of<AddOnService.IAddOnService>());

            var result = controller.Status().Result;

            Assert.IsTrue(result.GetType().FullName.StartsWith("System.Web.Http.Results.OkNegotiatedContentResult"),
                          "Index should return Ok");
        }

        [TestMethod]
        public void StatusReturnsCorrectContent()
        {
            var loginService = Mock.Of<LoginService.IClientLoginService>(s =>
                s.CheckUsernameAvailabilityAsync(It.IsAny<LoginService.CheckUsernameAvailabilityRequest>()) ==
                    Task.FromResult(true)
            );
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.HealthCheckAsync() == Task.FromResult("Success")
            );
            var controller = new IndexController(loginService, addOnService);

            dynamic result = controller.Status().Result;
            dynamic content = result.Content;

            Assert.AreEqual("Ok", content.CurseRestProxy,
                "Status should return Ok for CurseRestPRoxy");
            Assert.AreEqual("Ok", content.LoginService,
                "Status should return Ok for the login service");
            Assert.AreEqual("Ok", content.AddOnService,
                "Status should return Ok for addon service");
        }

        [TestMethod]
        public void StatusReturnsDownWhenLoginServiceUnreachable()
        {
            var loginService = Mock.Of<LoginService.IClientLoginService>();
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.HealthCheckAsync() == Task.FromResult("Success")
            );
            var controller = new IndexController(loginService, addOnService);

            Mock.Get(loginService).
                Setup(s => s.CheckUsernameAvailabilityAsync(It.IsAny<LoginService.CheckUsernameAvailabilityRequest>())).
                Throws<EndpointNotFoundException>();

            dynamic result = controller.Status().Result;
            dynamic content = result.Content;

            Assert.AreEqual("Down", content.LoginService,
                "Status should return Down for the login service when it is unreachable");
        }

        [TestMethod]
        public void StatusReturnsErrorWhenLoginServiceThrowsFault()
        {
            var loginService = Mock.Of<LoginService.IClientLoginService>();
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.HealthCheckAsync() == Task.FromResult("Success")
            );
            var controller = new IndexController(loginService, addOnService);

            Mock.Get(loginService).
                Setup(s => s.CheckUsernameAvailabilityAsync(It.IsAny<LoginService.CheckUsernameAvailabilityRequest>())).
                Throws<FaultException>();

            dynamic result = controller.Status().Result;
            dynamic content = result.Content;

            Assert.AreEqual("Error", content.LoginService,
                "Status should return Error for the login service when it throws a fault");
        }

        [TestMethod]
        public void StatusReturnsDownWhenAddOnServiceUnreachable()
        {
            var loginService = Mock.Of<LoginService.IClientLoginService>(s =>
                s.CheckUsernameAvailabilityAsync(It.IsAny<LoginService.CheckUsernameAvailabilityRequest>()) ==
                    Task.FromResult(true)
            );
            var addOnService = Mock.Of<AddOnService.IAddOnService>();
            var controller = new IndexController(loginService, addOnService);

            Mock.Get(addOnService).
                Setup(s => s.HealthCheckAsync()).
                Throws<EndpointNotFoundException>();

            dynamic result = controller.Status().Result;
            dynamic content = result.Content;

            Assert.AreEqual("Down", content.AddOnService,
                "Status should return Down for the add on service when it is unreachable");
        }

        [TestMethod]
        public void StatusReturnsErrorWhenAddOnServiceThrowsFault()
        {
            var loginService = Mock.Of<LoginService.IClientLoginService>(s =>
                s.CheckUsernameAvailabilityAsync(It.IsAny<LoginService.CheckUsernameAvailabilityRequest>()) ==
                    Task.FromResult(true)
            );
            var addOnService = Mock.Of<AddOnService.IAddOnService>();
            var controller = new IndexController(loginService, addOnService);

            Mock.Get(addOnService).
                Setup(s => s.HealthCheckAsync()).
                Throws<FaultException>();

            dynamic result = controller.Status().Result;
            dynamic content = result.Content;

            Assert.AreEqual("Error", content.AddOnService,
                "Status should return Error for the add on service when it throws a fault");
        }

        [TestMethod]
        public void StatusReturnsResultWhenAddOnServiceNotSuccess()
        {
            var loginService = Mock.Of<LoginService.IClientLoginService>(s =>
                s.CheckUsernameAvailabilityAsync(It.IsAny<LoginService.CheckUsernameAvailabilityRequest>()) ==
                    Task.FromResult(true)
            );
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.HealthCheckAsync() == Task.FromResult("SomeError")
            );
            var controller = new IndexController(loginService, addOnService);

            dynamic result = controller.Status().Result;
            dynamic content = result.Content;

            Assert.AreEqual("SomeError", content.AddOnService,
                "Status should return result for the add on service when it is not success");
        }
    }
}
