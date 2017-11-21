using System.Threading.Tasks;
using Microsoft.Owin;
using System.Linq;
using Ninject;
using Cosella.Framework.Core.Logging;
using Cosella.Framework.Core.Hosting;

namespace Cosella.Framework.Core.Authentication
{
    public class AuthenticationMiddleware : OwinMiddleware
    {
        private readonly ILogger _log;
        private readonly ITokenManager _tokenManager;
        private readonly HostedServiceConfiguration _serviceConfiguration;

        private readonly string[] tokenFieldNames = { "jwt", "token", "api_token", "api_key", "X-ApiKey" };

        public AuthenticationMiddleware(OwinMiddleware next, IKernel kernel)
            : base(next)
        {
            _log = kernel.Get<ILogger>();
            _tokenManager = kernel.Get<ITokenManager>();
            _serviceConfiguration = kernel.Get<HostedServiceConfiguration>();

        }

        public async override Task Invoke(IOwinContext context)
        {
            var token = tokenFieldNames
                .Select(n => context.Request.Query.Get(n))
                .FirstOrDefault(p => p != null);

            if (token == null)
            {
                token = tokenFieldNames
                    .Select(n => context.Request.Headers.Get(n))
                    .FirstOrDefault(p => p != null);
            }

            if (token != null)
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
