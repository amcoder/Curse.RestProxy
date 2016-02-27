using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Curse.RestProxy.Http
{
    /// <summary>
    /// Wraps an action result and executes the specified action after executing the result.
    /// </summary>
    public class WrappedActionResult: IHttpActionResult
    {
        private IHttpActionResult InnerResult { get; set; }
        private Action<HttpResponseMessage> Action { get; set; }

        public WrappedActionResult(IHttpActionResult innerResult, Action<HttpResponseMessage> action)
        {
            InnerResult = innerResult;
            Action = action;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await InnerResult.ExecuteAsync(cancellationToken);

            if(Action != null)
                Action(response);

            return response;
        }
    }
}