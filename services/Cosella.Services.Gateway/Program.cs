using Cosella.Framework.Core;
using Cosella.Framework.Extensions.Authentication;
using Cosella.Framework.Extensions.Authentication.Default;
using Cosella.Framework.Extensions.Gateway;
using System.Collections.Generic;

namespace Cosella.Services.Gateway
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return MicroService

                // Create the service
                .Configure(config =>
                {
                    // Gateway service basic config
                    config.ServiceName = "Gateway";
                    config.ServiceDisplayName = "Cosella Gateway";
                    config.ServiceDescription = "API gateway for Cosella framework";

                    // Configre the API location
                    config.RestApiPort = 5000;

                    // Turn off service discovery
                    //config.DisableRegistration = true;
                    //config.DisableServiceDiscovery = true;
                })
                // Use default gateway (No configuration available yet)
                .WithGateway(config =>
                {
                    // We're not interested in the gateway endpoint or any service managment facility
                    config.DisableServiceManager = true;
                })
                // Use the built in simple authenticator (inject your IAuthentictaor here)
                .WithAuthentication(config =>
                {
                    config.EnableSimpleTokenController = true;
                    config.EnableSimpleUserManager = true;
                    config.SimpleUserManagerSeedUsers = new List<User>()
                    {
                        new User("admin", "admin", new [] {"Authentication:Admin", "Authentication" })
                    };
                })
                .Run();
        }
    }
}