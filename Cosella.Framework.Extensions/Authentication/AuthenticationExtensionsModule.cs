using Cosella.Framework.Extensions.Authentication.Default;
using Ninject.Modules;

namespace Cosella.Framework.Extensions.Authentication
{
    public class AuthenticationExtensionsModule : NinjectModule
    {
        private readonly string _jwtSecret;
        private readonly IAuthenticator _authenticator;

        public AuthenticationExtensionsModule(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        public AuthenticationExtensionsModule(string jwtSecret)
        {
            _jwtSecret = jwtSecret;
        }

        public override void Load()
        {
            if (_authenticator != null)
            {
                Bind<IAuthenticator>().ToConstant(_authenticator).InSingletonScope();
            }
            else
            {
                Bind<IAuthenticator>().ToConstant(new DefaultAuthenticator(_jwtSecret)).InSingletonScope();
            }
        }
    }
}