using Cosella.Framework.Extensions.Authentication.Default;
using Cosella.Framework.Extensions.Authentication.Interfaces;
using Ninject.Modules;
using System;

namespace Cosella.Framework.Extensions.Authentication
{
    public class AuthenticationExtensionsModule : NinjectModule
    {
        private readonly AuthenticationConfiguration _config;
        private readonly Type _authenticatorType;
        private readonly Type _tokenHandlerType;

        public AuthenticationExtensionsModule(AuthenticationConfiguration config, Type authenticatorType = null, Type tokenHandlerType = null)
        {
            if (authenticatorType != null && typeof(IAuthenticator).IsAssignableFrom(authenticatorType) == false)
            {
                throw new ArgumentException($"Authenticator type {authenticatorType.Name} does not implement {nameof(IAuthenticator)}");
            }
            if (tokenHandlerType != null && typeof(ITokenHandler).IsAssignableFrom(tokenHandlerType) == false)
            {
                throw new ArgumentException($"TokenHandler type {tokenHandlerType.Name} does not implement {nameof(ITokenHandler)}");
            }

            _config = config;
            _authenticatorType = authenticatorType;
            _tokenHandlerType = tokenHandlerType;
        }

        public override void Load()
        {
            if (_authenticatorType != null) Bind<IAuthenticator>().To(_authenticatorType).InSingletonScope();
            else Bind<IAuthenticator>().To<DefaultAuthenticator>().InSingletonScope();

            if (_tokenHandlerType != null) Bind<ITokenHandler>().To(_tokenHandlerType).InSingletonScope();
            else Bind<ITokenHandler>().To<DefaultTokenHandler>().InSingletonScope();

            Bind<AuthenticationConfiguration>().ToMethod(context => _config);
            Bind<IUserManager>().To<SimpleUserManager>().InSingletonScope();

            if (_config.EnableSimpleUserManager) Bind<SimpleUserController>().To<SimpleUserController>();
            if (_config.EnableSimpleTokenController) Bind<SimpleTokenController>().To<SimpleTokenController>();
        }
    }
}