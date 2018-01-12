using Cosella.Framework.Core;

namespace Cosella.Framework.Extensions.Gateway
{
    public static class GatewayExtensions
    {
        public static MicroService WithGateway(this MicroService microservice)
        {
            microservice.Configuration.Modules.Add(new GatewayExtensionsModule());
            return microservice;
        }
    }
}