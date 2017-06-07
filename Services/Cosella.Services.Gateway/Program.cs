namespace Cosella.Services.Gateway
{
    using Core;

    internal class Program
    {
        private static int Main(string[] args)
        {
            return MicroService
                .ImplementGateway()
                .Run(config =>
                {
                    config.ServiceName = "Gateway";
                    config.ServiceDisplayName = "Cosella Gateway";
                    config.ServiceDescription = "API gateway for Cosella framework";

                    config.RestApiPort = 5000;
                });
        }
    }
}