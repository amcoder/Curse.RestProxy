using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Curse.RestProxy.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Curse.RestProxy.Tests.Controllers
{
    [TestClass]
    public class AddOnControllerDescriptionTests
    {
        [TestMethod]
        public void DescriptionReturnsOkWhenAddOnFound()
        {
            var description = "description";
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.v2GetAddOnDescriptionAsync(1) == Task.FromResult(description)
            );

            var controller = new AddOnController(addOnService);

            var result = controller.Description(1).Result;

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>),
                "Description should return Ok when the addon is found");
        }

        [TestMethod]
        public void DescriptionReturnsResultFromAddOnService()
        {
            var description = "description";
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.v2GetAddOnDescriptionAsync(1) == Task.FromResult(description)
            );

            var controller = new AddOnController(addOnService);

            var result = controller.Description(1).Result as OkNegotiatedContentResult<string>;

            Assert.AreEqual(description, result.Content,
                "Description should return result from the addon service");
        }

        [TestMethod]
        public void DescriptionReturnsNotFoundWhenAddOnDoesNotExist()
        {
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.v2GetAddOnDescriptionAsync(1) == Task.FromResult((string)null)
            );

            var controller = new AddOnController(addOnService);

            var result = controller.Description(1).Result;

            Assert.IsInstanceOfType(result, typeof(NotFoundResult),
                "Description should return not found when addon does not exist");
        }
    }
}
