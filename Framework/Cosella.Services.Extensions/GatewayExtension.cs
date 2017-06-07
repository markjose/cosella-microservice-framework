using Cosella.Services.Core;

namespace Cosella.Services.Extensions
{
    public static class GatewayExtension
    {
        public static MicroService ImplementGateway(this MicroService microservice)
        {
            return microservice;
        }
    }
}