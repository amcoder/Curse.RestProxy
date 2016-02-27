using System;
using System.Web.Http.Results;
using Curse.RestProxy.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Curse.RestProxy.Tests.Controllers
{
    [TestClass]
    public class IndexControllerIndexTests
    {
        [TestMethod]
        public void IndexReturnsOk()
        {
            var controller = new IndexController();

            var result = controller.Index();

            Assert.IsTrue(result.GetType().FullName.StartsWith("System.Web.Http.Results.OkNegotiatedContentResult"),
                "Index should return Ok");
        }

        [TestMethod]
        public void IndexReturnsCorrectResult()
        {
            var controller = new IndexController();

            dynamic result = controller.Index();

            Assert.IsNotNull(result.Content,
                "The content should not be null");
            Assert.IsNotNull(result.Content.Website,
                "The website link should not be null");
            Assert.AreEqual("http://github.com/amiller/Curse.RestProxy", result.Content.Website,
                "The website link should point to the correct location");
        }
    }
}
