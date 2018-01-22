﻿using Cosella.Framework.Core;
using Ninject;
using Owin;

namespace Cosella.Framework.Extensions.Authentication
{
    public static class AuthenticationExtensions
    {
        public static MicroService WithAuthentication(this MicroService microservice)
        {
            microservice.Configuration.Modules.Add(new AuthenticationExtensionsModule());
            microservice.Configuration.Middleware.Add(UseAuthentication);
            return microservice;
        }

        public static MicroService WithAuthentication<T>(this MicroService microservice) where T : IAuthenticator
        {
            microservice.Configuration.Modules.Add(new AuthenticationExtensionsModule(typeof(T)));
            microservice.Configuration.Middleware.Add(UseAuthentication);
            return microservice;
        }

        internal static IAppBuilder UseAuthentication(IAppBuilder app, IKernel kernel)
        {
            return app.Use<AuthenticationMiddleware>(kernel);
        }
    }
}