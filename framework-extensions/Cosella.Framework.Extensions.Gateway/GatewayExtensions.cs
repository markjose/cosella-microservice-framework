using Cosella.Framework.Core;
using System;

namespace Cosella.Framework.Extensions.Gateway
{
    public static class GatewayExtensions
    {
        public static MicroService WithGateway(this MicroService microservice, Action<GatewayConfiguration> configurator = null)
        {
            var configuration = new GatewayConfiguration();
            configurator?.Invoke(configuration);

            microservice.Configuration.Modules.Add(new GatewayExtensionsModule(configuration));
            return microservice;
        }
    }
}