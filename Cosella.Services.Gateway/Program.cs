namespace Cosella.Services.Gateway
{
    using Cosella.Framework.Core.Authentication;
    using Cosella.Framework.Extensions.Models;
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
                .AddAuthentication(config =>
                {
                    config.AuthenticationType = AuthenticationType.Jwt;
                    config.OnAuthenticate = OnAuthenticate;

                    config.Jwt.Secret = "nf42v97n24nn34589fcco3mjcfjv49vhcp93x9unv84bxv05jv0wm";
                })
                .AddGateway()
                .Run();
        }

        private static Framework.Core.Authentication.AuthenticatedUser OnAuthenticate(string username, string password)
        {
            return username == "test" && password == "test"
            ? new AuthenticatedUser
            {
                Username = "test",
                Name = "Test User",
                Roles = new[] { "_all_" }
            } : null;
        }
    }
}