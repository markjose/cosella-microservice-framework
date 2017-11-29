using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    internal class DefaultAuthenticator : IAuthenticator
    {
        private readonly IUserManager _users;

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

        public DefaultAuthenticator(IUserManager users)
        {
            _users = users;
        }

        public ClaimsPrincipal PrincipleFromToken(string token)
        {
            var parts = token.Split(':');

            if (parts.Length != 2) return PrincipleForGuest();
            if (parts.Any(p => string.IsNullOrWhiteSpace(p))) return PrincipleForGuest();

            var user = _users.Authenticate(parts[0], parts[1]);
            if (user == null) return PrincipleForGuest();

            return PrincipleFromUser(user);
        }

        private ClaimsPrincipal PrincipleForGuest()
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, "__guest"),
                new Claim(ClaimTypes.Authentication, "true", ClaimValueTypes.Boolean)
            };
            var identity = new ClaimsIdentity(claims);
            return new ClaimsPrincipal(identity);
        }

        private ClaimsPrincipal PrincipleFromUser(User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Authentication, "true", ClaimValueTypes.Boolean)
            };
            var identity = new ClaimsIdentity(claims.Concat(user.Roles.Select(r => new Claim(ClaimTypes.Role, r))));
            return new ClaimsPrincipal(identity);
        }

        public bool AuthenticateInRole(IPrincipal user, string[] roles, dynamic contextData = null)
        {
            if (_users.Empty) return true;

            // contextData is ignored with this implementation so if its specified just return false
            if (contextData != null) return false;

            return roles.All(r => user.IsInRole(r));
        }
    }
}
