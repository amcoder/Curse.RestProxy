using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Curse.RestProxy.Http
{
    public static class HttpResponseMessageExtensions
    {
        public static void AddWwwAuthenticateTokenHeader(this HttpResponseMessage response)
        {
            if(!response.HasWwwAuthenticateTokenHeader())
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Token", "realm=\"api\""));
        }

        public static bool HasWwwAuthenticateTokenHeader(this HttpResponseMessage response)
        {
            return response.Headers.WwwAuthenticate.Any(h => h.Scheme == "Token");
        }
    }
}