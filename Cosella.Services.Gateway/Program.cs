using Cosella.Framework.Core;
using Cosella.Framework.Extensions.Authentication;
using Cosella.Framework.Extensions.Gateway;

namespace Cosella.Services.Gateway
{
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
                .AddAuthentication("nf42v97n24nn34589fcco3mjcfjv49vhcp93x9unv84bxv05jv0wm")
                .Run();
        }
    }
}