using Cosella.Framework.Core;
using Cosella.Framework.Extensions.ApplicationHost;
using Cosella.Framework.Extensions.Authentication;
using Cosella.Framework.Extensions.Gateway;

namespace Cosella.Services.Gateway
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return MicroService

                // Create the service
                .Create(config =>
                {
                    // Gateway service basic config
                    config.ServiceName = "Gateway";
                    config.ServiceDisplayName = "Cosella Gateway";
                    config.ServiceDescription = "API gateway for Cosella framework";

                    // Configre the API location
                    config.RestApiPort = 5000;

                    // Turn off service discovery
                    config.DisableRegistration = true;
                    config.DisableServiceDiscovery = true;
                })

                // Use default gateway (No configuration available yet)
                .AddGateway()

                // Use the built in simple authenticator (inject your IAuthentictaor here)
                .AddAuthentication()

                // Set up some example applications ho host
                .AddApplicationHost(config =>
                {
                    // Deploy and configure apps via the API
                    config.EnableApplicationManagerApi = true;

                    // Simple Add
                    config.Applications.Add("Example Application 1", "app1");
                    config.Applications.Add("Example Application 2", "app2", true);

                    // Fully configured Add
                    config.Applications.Add(new HostedApplicationConfiguration
                    {
                        Name = "Example Application 3",
                        Aliases = new[] { "app3" },
                        ApplicationType = HostedApplicationTypes.React,
                        ApplicationRoot = "./Apps/app3/build"
                    });
                })
                .Run();
        }
    }
}