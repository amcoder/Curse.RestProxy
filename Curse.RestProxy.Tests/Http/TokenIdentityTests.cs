using System;
using Curse.RestProxy.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Curse.RestProxy.Tests.Http
{
    [TestClass]
    public class TokenIdentityTests
    {
        [TestMethod]
        public void ConstructorSetsUserId()
        {
            var identity = new TokenIdentity(1, "token");

            Assert.AreEqual(1, identity.UserId,
                "The constructor should set the UserId");
        }

        [TestMethod]
        public void ConstructorSetsToken()
        {
            var identity = new TokenIdentity(1, "token");

            Assert.AreEqual("token", identity.Token,
                "The constructor should set the Token");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
            "Constructing with a null token should throw ArgumentNullException")]
        public void ConstructorThrowsWhenTokenNull()
        {
            new TokenIdentity(1, null);
        }

        [TestMethod]
        public void NameIsUserId()
        {
            var identity = new TokenIdentity(1, "token");

            Assert.AreEqual("1", identity.Name,
                "Name should be the string represenation of UserId");
        }

        [TestMethod]
        public void AuthenticationTypeIsToken()
        {
            var identity = new TokenIdentity(1, "token");

            Assert.AreEqual("Token", identity.AuthenticationType,
                "AuthenticationType should be \"Token\"");
        }

        [TestMethod]
        public void IsAuthenticatedIsTrue()
        {
            var identity = new TokenIdentity(1, "token");

            Assert.IsTrue(identity.IsAuthenticated,
                "IsAuthenticated should be true.");
        }
    }
}
