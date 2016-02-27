using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace Curse.RestProxy.Serialization
{
    public class SnakeCasePropertyNamesContractResolver: DefaultContractResolver
    {
        private Regex upperLowerRegex = new Regex("([A-Z]+)([A-Z][a-z])", RegexOptions.Compiled);
        private Regex lowerUpperRegex = new Regex("([a-z])([A-Z])", RegexOptions.Compiled);

        protected override string ResolvePropertyName(string propertyName)
        {
            propertyName = upperLowerRegex.Replace(propertyName, "$1_$2");
            propertyName = lowerUpperRegex.Replace(propertyName, "$1_$2");

            return propertyName.ToLower();
        }
    }
}