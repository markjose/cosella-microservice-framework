using Cosella.Framework.Core.Logging;
using Cosella.Framework.Extensions.Authentication.Interfaces;
using Microsoft.Owin;
using Ninject;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Cosella.Framework.Extensions.Authentication
{
    public class AuthenticationMiddleware : OwinMiddleware
    {
        private readonly ILogger _log;
        private readonly IAuthenticator _authenticator;
        private readonly ITokenHandler _tokenHandler;

        public AuthenticationMiddleware(OwinMiddleware next, IKernel kernel) : base(next)
        {
            _log = kernel.Get<ILogger>();
            _authenticator = kernel.Get<IAuthenticator>();
            _tokenHandler = kernel.Get<ITokenHandler>();
        }

        public override async Task Invoke(IOwinContext context)
        {
            var token = GetTokenFromRequest(context.Request);
            if (string.IsNullOrWhiteSpace(token) == false)
            {
                var claims = await _tokenHandler.ClaimsFromToken(token);
                var identity = await _tokenHandler.IdentityFromClaims(claims);
                if (string.IsNullOrWhiteSpace(identity) == false)
                {
                    var user = await _authenticator.UserFromIdentity(identity);
                    if (user != null) context.Authentication.User = PrincipleFromUser(user);
                }
            }
            await Next.Invoke(context);
        }

        private ClaimsPrincipal PrincipleFromUser(IUser user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Identity),
                new Claim(ClaimTypes.Authentication, "true", ClaimValueTypes.Boolean)
            };
            var identity = new ClaimsIdentity(claims.Concat(user.Roles.Select(r => new Claim(ClaimTypes.Role, r))));
            return new ClaimsPrincipal(identity);
        }

        private string GetTokenFromRequest(IOwinRequest request)
        {
            var token = "";

            if (_tokenHandler.TokenSources == null) return token;

            foreach (var source in _tokenHandler.TokenSources)
            {
                if (string.IsNullOrWhiteSpace(source.Name)) continue;

                if (source.Type == AuthenticationTokenSourceType.Header)
                {
                    token = request.Headers.Get(source.Name);
                    if (!string.IsNullOrWhiteSpace(token)) break;
                }
                else if (source.Type == AuthenticationTokenSourceType.Url)
                {
                    var queryParams = HttpUtility.ParseQueryString(request.Uri.Query);
                    token = queryParams.Get(source.Name);
                    if (!string.IsNullOrWhiteSpace(token)) break;
                }
            }
            return token;
        }
    }
}
