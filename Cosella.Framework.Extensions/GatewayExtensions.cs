using Cosella.Framework.Core;
using System.Linq;

namespace Cosella.Framework.Extensions
{
    public static class GatewayExtensions
    {
        public static MicroService AddGateway(this MicroService microservice)
        {
            microservice.Configuration.Modules.Add(new GatewayExtensionsModule());
            return microservice;
        }
    }
}