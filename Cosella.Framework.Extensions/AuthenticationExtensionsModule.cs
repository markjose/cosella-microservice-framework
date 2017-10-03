using Cosella.Framework.Core.Authentication;
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
        }
    }
}