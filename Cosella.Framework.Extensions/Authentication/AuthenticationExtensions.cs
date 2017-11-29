using Cosella.Framework.Core;
using Cosella.Framework.Core.Hosting;
using Ninject;
using Owin;

namespace Cosella.Framework.Extensions.Authentication
{
    public static class AuthenticationExtensions
    {
        public static MicroService AddAuthentication(this MicroService microservice, string jwtSecret)
        {
            microservice.Configuration.Modules.Add(new AuthenticationExtensionsModule(jwtSecret));
            microservice.Configuration.Middleware.Add(UseAuthentication);
            return microservice;
        }

        public static MicroService AddAuthentication<T>(this MicroService microservice, T authenticator) where T : IAuthenticator
        {
            microservice.Configuration.Modules.Add(new AuthenticationExtensionsModule(authenticator));
            microservice.Configuration.Middleware.Add(UseAuthentication);
            return microservice;
        }

        public static IAppBuilder UseAuthentication(IAppBuilder app, IKernel kernel)
        {
            return app.Use<AuthenticationMiddleware>(kernel);
        }
    }
}