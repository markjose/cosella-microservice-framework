using System.Threading.Tasks;
using Microsoft.Owin;
using Ninject;
using Cosella.Framework.Core.Logging;
using System.Web;
using Cosella.Framework.Core.Hosting;

namespace Cosella.Framework.Extensions.Authentication
{
    public class AuthenticationMiddleware : OwinMiddleware
    {
        private readonly ILogger _log;
        private readonly IAuthenticator _authenticator;

        public AuthenticationMiddleware(OwinMiddleware next, IKernel kernel) : base(next)
        {
            _log = kernel.Get<ILogger>();
            _authenticator = kernel.Get<IAuthenticator>();
        }

        public override async Task Invoke(IOwinContext context)
        {
            var token = GetTokenFromRequest(context.Request);
            var user = _authenticator.PrincipleFromToken(token ?? "");
            if (user == null)
            {
                await Next.Invoke(context);
                return;
            }

            context.Authentication.User = user;
            _log.Debug($"Authenticated as '{user.Identity.Name}'");
            await Next.Invoke(context);
        }

        private string GetTokenFromRequest(IOwinRequest request)
        {
            var token = "";

            if (_authenticator.TokenSources == null) return token;

            foreach (var source in _authenticator.TokenSources)
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
