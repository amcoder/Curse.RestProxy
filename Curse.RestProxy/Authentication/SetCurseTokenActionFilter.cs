using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Curse.RestProxy.AddOnService;

namespace Curse.RestProxy.Authentication
{
    /// <summary>
    /// Sets the token for the Curse WCF services to the authentication token from the
    /// authorization header.
    /// </summary>
    public class SetCurseTokenActionFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            var identity = actionContext.RequestContext.Principal.Identity as TokenIdentity;

            if(identity == null)
                return;

            var dependencyResolver = actionContext.Request.GetDependencyScope();
            var addOnService = dependencyResolver.GetService(typeof(IAddOnService)) as AddOnServiceClient;

            addOnService.ChannelFactory.Endpoint.Behaviors.Add(new TokenEndpointBehavior(identity));
        }
    }
}