using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Extensions.Authentication.Default;
using Ninject.Modules;

namespace Cosella.Framework.Extensions.Authentication
{
    public class AuthenticationExtensionsModule : NinjectModule
    {
        private readonly IAuthenticator _authenticator;

        public AuthenticationExtensionsModule(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        public AuthenticationExtensionsModule()
        {
        }

        public override void Load()
        {
            if (_authenticator != null)
            {
                Bind<IAuthenticator>()
                    .ToConstant(_authenticator)
                    .InSingletonScope();
            }
            else
            {
                Bind<IUserManager>()
                    .To<UserManager>()
                    .InSingletonScope();

                Bind<IAuthenticator>()
                    .To<DefaultAuthenticator>()
                    .InSingletonScope();
            }
        }
    }
}