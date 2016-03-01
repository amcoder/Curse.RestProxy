using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Curse.RestProxy.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Curse.RestProxy.Tests.Controllers
{
    [TestClass]
    public class AddOnFilesControllerChangelogTests
    {
        [TestMethod]
        public void ChangelogReturnsOkWhenFileFound()
        {
            var changelog = "changes";
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.v2GetChangeLogAsync(1, 2) == Task.FromResult(changelog)
            );

            var controller = new AddOnFilesController(addOnService);

            var result = controller.Changelog(1, 2).Result;

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>),
                "Get should return Ok when the file is found");
        }

        [TestMethod]
        public void ChangelogReturnsResultFromAddOnService()
        {
            var changelog = "changes";
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.v2GetChangeLogAsync(1, 2) == Task.FromResult(changelog)
            );

            var controller = new AddOnFilesController(addOnService);

            var result = controller.Changelog(1, 2).Result as OkNegotiatedContentResult<string>;

            Assert.AreEqual(changelog, result.Content,
                "Get should return result from the addon service");
        }

        [TestMethod]
        public void ChangelogReturnsNotFoundWhenFileDoesNotExist()
        {
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.v2GetChangeLogAsync(1, 2) == Task.FromResult((string)null)
            );

            var controller = new AddOnFilesController(addOnService);

            var result = controller.Changelog(1, 2).Result;

            Assert.IsInstanceOfType(result, typeof(NotFoundResult),
                "Get should return not found when file does not exist");
        }
    }
}
