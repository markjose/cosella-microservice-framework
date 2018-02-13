using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosella.Framework.Extensions.Authentication.Interfaces
{
    public interface ITokenHandler
    {
        IEnumerable<AuthenticationTokenSource> TokenSources { get; }

        Task<Dictionary<string, object>> ClaimsFromToken(string token);
        Task<string> CreateToken(Dictionary<string, object> claims);
        Task<string> IdentityFromClaims(Dictionary<string, object> claims);
    }
}
