using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http.Filters;
using Curse.RestProxy.Http;

namespace Curse.RestProxy.Authentication
{
    /// <summary>
    /// Handles a FaultCode of "FailedAuthentication" from the Curse WCF services by returning
    /// a 401 Unauthorized response.
    /// </summary>
    public class CurseFailedAuthenticationExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var faultException = context.Exception as FaultException;
            if(faultException == null || faultException.Code.Name != "FailedAuthentication")
                return;

            context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            context.Response.AddWwwAuthenticateTokenHeader();
        }
    }
}