using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Cosella.Framework.Extensions.Authentication
{
    public interface IAuthenticator
    {
        IEnumerable<AuthenticationTokenSource> TokenSources { get; }
        ClaimsPrincipal PrincipleFromToken(string token);
        bool AuthenticateInRole(IPrincipal user, string[] roles, dynamic contextData = null);
    }
}
