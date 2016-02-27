using System;
using System.Security.Principal;

namespace Curse.RestProxy.Authentication
{
    /// <summary>
    /// Provides an identity based on an access token
    /// </summary>
    public class TokenIdentity: IIdentity
    {
        public int UserId { get; private set; }
        public string Token { get; private set; }

        public TokenIdentity(int userId, string token)
        {
            if(token == null)
                throw new ArgumentNullException("token");

            UserId = userId;
            Token = token;
        }

        public string AuthenticationType
        {
            get
            {
                return "Token";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public string Name
        {
            get
            {
                return UserId.ToString();
            }
        }
    }
}