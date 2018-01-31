using Cosella.Framework.Core;
using Cosella.Framework.Extensions.Authentication.Interfaces;
using Ninject;
using Owin;
using System;

namespace Cosella.Framework.Extensions.Authentication
{
    public static class AuthenticationExtensions
    {
        private static MicroService Init(MicroService microservice, Action<AuthenticationConfiguration> configurator, Type authenticatorType, Type tokenHandlerType)
        {
            var configuration = new AuthenticationConfiguration();
            configurator?.Invoke(configuration);

            microservice.Configuration.Modules.Add(new AuthenticationExtensionsModule(configuration, authenticatorType, tokenHandlerType));
            microservice.Configuration.Middleware.Add(UseAuthentication);
            return microservice;
        }

        public static MicroService WithAuthentication(this MicroService microservice, Action<AuthenticationConfiguration> configurator = null)
        {
            return Init(microservice, configurator, null, null);
        }

        public static MicroService WithAuthentication<TAuthenticator>(this MicroService microservice, Action<AuthenticationConfiguration> configurator = null) 
            where TAuthenticator : IAuthenticator
        {
            return Init(microservice, configurator, typeof(TAuthenticator), null);
        }

        public static MicroService WithAuthentication<TAuthenticator, TTokenHandler>(this MicroService microservice, Action<AuthenticationConfiguration> configurator = null)
            where TAuthenticator : IAuthenticator
            where TTokenHandler : ITokenHandler
        {
            return Init(microservice, configurator, typeof(TAuthenticator), typeof(TTokenHandler));
        }

        internal static IAppBuilder UseAuthentication(IAppBuilder app, IKernel kernel)
        {
            return app.Use<AuthenticationMiddleware>(kernel);
        }
    }
}