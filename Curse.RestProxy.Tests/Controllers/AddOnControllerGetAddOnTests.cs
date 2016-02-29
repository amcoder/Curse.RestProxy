using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Curse.RestProxy.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Curse.RestProxy.Tests.Controllers
{
    [TestClass]
    public class AddOnControllerGetAddOnTests
    {
        [TestMethod]
        public void GetAddOnReturnsOkWhenAddOnFound()
        {
            var addOn = new AddOnService.AddOn();
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.GetAddOnAsync(1) == Task.FromResult(addOn)
            );

            var controller = new AddOnController(addOnService);

            var result = controller.GetAddOn(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AddOnService.AddOn>),
                "GetAddOn should return Ok when the addon is found");
        }

        [TestMethod]
        public void GetAddOnReturnsResultFromAddOnService()
        {
            var addOn = new AddOnService.AddOn();
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.GetAddOnAsync(1) == Task.FromResult(addOn)
            );

            var controller = new AddOnController(addOnService);

            var result = controller.GetAddOn(1).Result as OkNegotiatedContentResult<AddOnService.AddOn>;

            Assert.AreEqual(addOn, result.Content,
                "GetAddOn should return result from the addon service");
        }

        [TestMethod]
        public void GetAddOnReturnsNotFoundWhenAddOnDoesNotExist()
        {
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.GetAddOnAsync(1) == Task.FromResult((AddOnService.AddOn)null)
            );

            var controller = new AddOnController(addOnService);

            var result = controller.GetAddOn(1).Result;

            Assert.IsInstanceOfType(result, typeof(NotFoundResult),
                "GetAddOn should return not found when addon does not exist");
        }
    }
}
