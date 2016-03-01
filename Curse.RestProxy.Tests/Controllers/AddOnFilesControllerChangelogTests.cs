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

            Assert.IsTrue(result.GetType().FullName.StartsWith("System.Web.Http.Results.OkNegotiatedContentResult"),
                "Changelog should return Ok when the file is found");
        }

        [TestMethod]
        public void ChangelogReturnsResultFromAddOnService()
        {
            var changelog = "changes";
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.v2GetChangeLogAsync(1, 2) == Task.FromResult(changelog)
            );

            var controller = new AddOnFilesController(addOnService);

            dynamic result = controller.Changelog(1, 2).Result;

            Assert.AreEqual(changelog, result.Content.Changelog,
                "Changelog should return result from the addon service");
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
                "Changelog should return not found when file does not exist");
        }
    }
}
