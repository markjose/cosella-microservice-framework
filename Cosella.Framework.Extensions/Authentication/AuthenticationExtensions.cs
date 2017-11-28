using Cosella.Framework.Core;

namespace Cosella.Framework.Extensions.Authentication
{
    public static class AuthenticationExtensions
    {
        public static MicroService AddAuthentication(this MicroService microservice, string jwtSecret)
        {
            microservice.Configuration.Modules.Add(new AuthenticationExtensionsModule(jwtSecret));
            return microservice;
        }

        public static MicroService AddAuthentication<T>(this MicroService microservice, T authenticator) where T : IAuthenticator
        {
            microservice.Configuration.Modules.Add(new AuthenticationExtensionsModule(authenticator));
            return microservice;
        }
    }
}