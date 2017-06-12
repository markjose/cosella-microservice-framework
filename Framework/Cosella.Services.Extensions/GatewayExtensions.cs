using Cosella.Services.Core;

namespace Cosella.Services.Extensions
{
    public static class GatewayExtensions
    {
        public static MicroService AddGateway(this MicroService microservice)
        {
            microservice.Configuration.Modules.Add(new GatewayModule());
            return microservice;
        }
    }
}