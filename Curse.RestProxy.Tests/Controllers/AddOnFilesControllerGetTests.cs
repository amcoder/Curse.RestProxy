using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Curse.RestProxy.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Curse.RestProxy.Tests.Controllers
{
    [TestClass]
    public class AddOnFilesControllerGetTests
    {
        [TestMethod]
        public void GetReturnsOkWhenFileFound()
        {
            var file = new AddOnService.AddOnFile();
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.GetAddOnFileAsync(1, 2) == Task.FromResult(file)
            );

            var controller = new AddOnFilesController(addOnService);

            var result = controller.Get(1, 2).Result;

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AddOnService.AddOnFile>),
                "Get should return Ok when the file is found");
        }

        [TestMethod]
        public void GetReturnsResultFromAddOnService()
        {
            var file = new AddOnService.AddOnFile();
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.GetAddOnFileAsync(1, 2) == Task.FromResult(file)
            );

            var controller = new AddOnFilesController(addOnService);

            var result = controller.Get(1, 2).Result as OkNegotiatedContentResult<AddOnService.AddOnFile>;

            Assert.AreEqual(file, result.Content,
                "Get should return result from the addon service");
        }

        [TestMethod]
        public void GetReturnsNotFoundWhenFileDoesNotExist()
        {
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.GetAddOnFileAsync(1, 2) == Task.FromResult((AddOnService.AddOnFile)null)
            );

            var controller = new AddOnFilesController(addOnService);

            var result = controller.Get(1, 2).Result;

            Assert.IsInstanceOfType(result, typeof(NotFoundResult),
                "Get should return not found when file does not exist");
        }
    }
}
