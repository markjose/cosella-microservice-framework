using System.Threading.Tasks;
using Microsoft.Owin;
using System.Linq;
using Cosella.Framework.Extensions.Interfaces;

namespace Cosella.Framework.Extensions.Middleware
{
    public class AuthenticationMiddleware : OwinMiddleware
    {
        private readonly string[] tokenFieldNames = { "jwt", "token", "api_token", "api_key", "X-ApiKey" };
        private readonly ITokenManager _tokenManager;

        public AuthenticationMiddleware(OwinMiddleware next, ITokenManager tokenManager) : base(next)
        {
            _tokenManager = tokenManager;
        }

        public async override Task Invoke(IOwinContext context)
        {
            var token = tokenFieldNames
                .Select(n => context.Request.Query.Get(n))
                .FirstOrDefault(p => p != null);

            if(token == null)
            {
                token = tokenFieldNames
                    .Select(n => context.Request.Headers.Get(n))
                    .FirstOrDefault(p => p != null);
            }

            if(token != null)
            {
                var user = _tokenManager.Decode(token);
                if (user != null)
                {
                   //var controller = context.


                }
            }

            await Next.Invoke(context);
        }
    }
}
