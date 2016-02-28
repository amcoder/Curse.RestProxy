using System.Web.Http;
using Curse.RestProxy.Authentication;
using Curse.RestProxy.Serialization;
using Newtonsoft.Json.Converters;

namespace Curse.RestProxy
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configure routes
            config.MapHttpAttributeRoutes();

            // Configure json formatting
            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new SnakeCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            config.Formatters.Clear();
            config.Formatters.Add(jsonFormatter);

            // Configure authentication and authorization
            config.Filters.Add(new TokenAuthenticationFilter());
            config.Filters.Add(new AuthorizeAttribute());

            // Configure dependency injection
            DependencyContainerConfig.Configure(config);
        }
    }
}
