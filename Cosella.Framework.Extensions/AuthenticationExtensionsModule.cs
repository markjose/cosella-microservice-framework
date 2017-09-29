using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Extensions.Configuration;
using Cosella.Framework.Extensions.Interfaces;
using Cosella.Framework.Extensions.Managers;
using Cosella.Framework.Extensions.Middleware;
using Ninject.Modules;

namespace Cosella.Framework.Extensions
{
    public class AuthenticationExtensionsModule : NinjectModule
    {
        private readonly AuthenticationConfiguration _configuration;

        public AuthenticationExtensionsModule(AuthenticationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void Load()
        {
            Bind<AuthenticationConfiguration>().ToMethod(context => _configuration);
            Bind<AuthenticationMiddleware>();
            Bind<ITokenManager>().To<TokenManager>();
        }
    }
}