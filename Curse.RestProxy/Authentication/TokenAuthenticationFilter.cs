using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Curse.RestProxy.Http;

namespace Curse.RestProxy.Authentication
{
    /// <summary>
    /// Provides authentication based on a token in the Authorization header
    /// </summary>
    public class TokenAuthenticationFilter: IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                HttpRequestMessage request = context.Request;
                AuthenticationHeaderValue authorization = request.Headers.Authorization;

                if(authorization == null)
                    return;

                if(authorization.Scheme != "Token")
                    return;

                int userId;
                string token;
                if(!TryParseAuthorizationParameter(authorization.Parameter, out userId, out token))
                {
                    context.ErrorResult = new StatusCodeResult(System.Net.HttpStatusCode.Unauthorized, request);
                    return;
                }

                context.Principal = new GenericPrincipal(new TokenIdentity(userId, token), new[] { "User" });

                return;
            });
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = context.Result.With(response =>
            {
                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized &&
                    !response.Headers.WwwAuthenticate.Any((h) => h.Scheme == "Token"))
                {
                    response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Token", "realm=\"api\""));
                }
            });
            return Task.FromResult(0);
        }

        /// <summary>
        /// Try to parse an authorization header parameter of the form "userid:token".
        ///     userid must be an integer
        ///     token must be non-empty string
        /// </summary>
        private static bool TryParseAuthorizationParameter(string authParam, out int userId, out string token)
        {
            userId = 0;
            token = null;

            if(String.IsNullOrEmpty(authParam))
                return false;

            var parts = authParam.Split(':');
            if(parts.Length != 2)
                return false;

            if(!int.TryParse(parts[0], out userId))
                return false;

            token = parts[1];
            if(String.IsNullOrEmpty(token))
                return false;

            return true;
        }
    }
}