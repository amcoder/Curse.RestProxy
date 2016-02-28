using System.Collections.Generic;
using Curse.RestProxy.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Curse.RestProxy.Tests.Serialization
{
    [TestClass]
    public class SnakeCasePropertyNamesContractResolverTests
    {
        private IDictionary<string, string> TestCases = new Dictionary<string, string>
        {
            { "already_snake", "already_snake" },
            { "ALLUPPER", "allupper" },
            { "alllower", "alllower" },
            { "PascalCase", "pascal_case" },
            { "camelCase", "camel_case" },
            { "ACRONYMName", "acronym_name" },
            { "SomeACRONYM", "some_acronym" }
        };

        [TestMethod]
        public void ResolvesCorrectSnakeCaseNames()
        {
            var resolver = new SnakeCasePropertyNamesContractResolver();

            foreach(var kvp in TestCases)
            {
                var result = resolver.GetResolvedPropertyName(kvp.Key);
                Assert.AreEqual(kvp.Value, result,
                    "{0} should resolve to {1}", kvp.Key, kvp.Value);
            }
        }
    }
}
