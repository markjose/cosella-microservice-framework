using Cosella.Framework.Core;
using Cosella.Framework.Core.Authentication;
using System;

namespace Cosella.Framework.Extensions
{
    public static class AuthenticationExtensions
    {
        public static MicroService AddAuthentication(this MicroService microservice, Action<AuthenticationConfiguration> configurator = null)
        {
            var config = new AuthenticationConfiguration();
            configurator?.Invoke(config);
            microservice.Configuration.Modules.Add(new AuthenticationExtensionsModule(config));
            return microservice;
        }
    }
}