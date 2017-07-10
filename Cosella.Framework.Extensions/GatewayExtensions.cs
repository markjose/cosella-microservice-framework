namespace Cosella.Framework.Extensions
{
    using Core;

    public static class GatewayExtensions
    {
        public static MicroService AddGateway(this MicroService microservice)
        {
            microservice.Configuration.Modules.Add(new ExtensionsModule());
            return microservice;
        }
    }
}