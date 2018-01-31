using System.Collections.Generic;

namespace Cosella.Framework.Extensions.Authentication.Interfaces
{
    public interface ITokenHandler
    {
        IEnumerable<AuthenticationTokenSource> TokenSources { get; }

        Dictionary<string, object> ClaimsFromToken(string token);
        string CreateToken(Dictionary<string, object> claims);
        string IdentityFromClaims(Dictionary<string, object> claims);
    }
}
