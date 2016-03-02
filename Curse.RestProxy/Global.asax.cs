using System.Configuration;
using System.Web.Http;
using Microsoft.ApplicationInsights.Extensibility;

namespace Curse.RestProxy
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}