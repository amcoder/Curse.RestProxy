using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Curse.RestProxy.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Curse.RestProxy.Tests.Controllers
{
    [TestClass]
    public class AddOnFilesControllerIndexTests
    {
        [TestMethod]
        public void IndexReturnsOkWhenAddOnFound()
        {
            var files = new List<AddOnService.AddOnFile>();
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.GetAllFilesForAddOnAsync(1) == Task.FromResult(files)
            );

            var controller = new AddOnFilesController(addOnService);

            var result = controller.Index(1).Result;

            Assert.IsTrue(result.GetType().FullName.StartsWith("System.Web.Http.Results.OkNegotiatedContentResult"),
                "Index should return Ok when the addon is found");
        }

        [TestMethod]
        public void IndexReturnsResultFromAddOnService()
        {
            var files = new List<AddOnService.AddOnFile>();
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.GetAllFilesForAddOnAsync(1) == Task.FromResult(files)
            );

            var controller = new AddOnFilesController(addOnService);

            dynamic result = controller.Index(1).Result;

            Assert.AreEqual(files, result.Content.Files,
                "Index should return result from the addon service");
        }

        [TestMethod]
        public void IndexReturnsNotFoundWhenAddOnDoesNotExist()
        {
            var addOnService = Mock.Of<AddOnService.IAddOnService>(s =>
                s.GetAllFilesForAddOnAsync(1) == Task.FromResult((List<AddOnService.AddOnFile>)null)
            );

            var controller = new AddOnFilesController(addOnService);

            var result = controller.Index(1).Result;

            Assert.IsInstanceOfType(result, typeof(NotFoundResult),
                "Index should return not found when addon does not exist");
        }
    }
}
