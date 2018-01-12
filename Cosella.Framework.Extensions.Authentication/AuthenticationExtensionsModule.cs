using Cosella.Framework.Extensions.Authentication.Default;
using Ninject.Modules;
using System;

namespace Cosella.Framework.Extensions.Authentication
{
    public class AuthenticationExtensionsModule : NinjectModule
    {
        private readonly Type _authenticatorType;

        public AuthenticationExtensionsModule(Type authenticatorType)
        {
            if(!typeof(IAuthenticator).IsAssignableFrom(authenticatorType))
            {
                throw new ArgumentException($"Authenticator type {authenticatorType.Name} does not implement {nameof(IAuthenticator)}");
            }
            _authenticatorType = authenticatorType;
        }

        public AuthenticationExtensionsModule()
        {
        }

        public override void Load()
        {
            if (_authenticatorType != null)
            {
                Bind<IAuthenticator>()
                    .To(_authenticatorType)
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