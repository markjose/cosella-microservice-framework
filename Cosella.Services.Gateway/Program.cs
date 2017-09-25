﻿namespace Cosella.Services.Gateway
{
    using Framework.Core;
    using Framework.Extensions;

    internal class Program
    {
        private static int Main(string[] args)
        {
            return MicroService
                .Create(config =>
                {
                    config.ServiceName = "Gateway";
                    config.ServiceDisplayName = "Cosella Gateway";
                    config.ServiceDescription = "API gateway for Cosella framework";

                    config.RestApiPort = 5000;

                    config.DisableRegistration = true;
                    config.DisableServiceDiscovery = true;
                })
                .AddGateway()
                .Run();
        }
    }
}