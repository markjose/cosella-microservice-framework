using System;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    internal class DefaultAuthenticator : IAuthenticator
    {
        private readonly string _jwtSecret;

        public DefaultAuthenticator(string jwtSecret)
        {
            _jwtSecret = jwtSecret;
        }

        public string TokenFromUser(AuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        public AuthenticatedUser UserFromCredentials(string userId, string secret)
        {
            throw new NotImplementedException();
        }

        public AuthenticatedUser UserFromToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
