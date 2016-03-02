using System.Configuration;
using System.Web.Http;
using Microsoft.ApplicationInsights.Extensibility;

namespace Curse.RestProxy
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var insightsKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
            if(!string.IsNullOrEmpty(insightsKey))
                TelemetryConfiguration.Active.InstrumentationKey = insightsKey;

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}