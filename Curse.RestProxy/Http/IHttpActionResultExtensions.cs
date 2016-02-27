using System;
using System.Net.Http;
using System.Web.Http;

namespace Curse.RestProxy.Http
{
    public static class IHttpActionResultExtensions
    {
        /// <summary>
        /// Perform additional processing after the action result is executed
        /// </summary>
        public static IHttpActionResult With(this IHttpActionResult result, Action<HttpResponseMessage> action)
        {
            return new WrappedActionResult(result, action);
        }
    }
}