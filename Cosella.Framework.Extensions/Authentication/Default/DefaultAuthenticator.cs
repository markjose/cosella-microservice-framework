using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    internal class DefaultAuthenticator : IAuthenticator
    {
        private readonly string _jwtSecret;

        public IEnumerable<AuthenticationTokenSource> TokenSources => new[]
        {
            new AuthenticationTokenSource
            {
                Type = AuthenticationTokenSourceType.Header,
                Name = "X-ApiKey"
            },
            new AuthenticationTokenSource
            {
                Type = AuthenticationTokenSourceType.Url,
                Name = "apikey"
            },
        };

        public DefaultAuthenticator(string jwtSecret)
        {
            _jwtSecret = jwtSecret;
        }

        public ClaimsPrincipal PrincipleFromToken(string token)
        {
            var roles = new [] { "private", "granular" };
            var username = "test";

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Authentication, "true", ClaimValueTypes.Boolean)
            };

            var identity = new ClaimsIdentity(claims.Concat(roles.Select(r => new Claim(ClaimTypes.Role, r))));
            var principle = new ClaimsPrincipal(identity);

            return principle;
        }

        public bool AuthenticateInRole(IPrincipal user, string[] roles, dynamic contextData = null)
        {
            // contextData is ignored with this implementation so if its specified just return false
            if (contextData != null) return false;

            return roles.All(r => user.IsInRole(r));
        }
    }
}
