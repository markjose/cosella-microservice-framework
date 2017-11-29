using Cosella.Framework.Core;
using Cosella.Framework.Extensions.Authentication;

namespace Cosella.Services.User
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return MicroService
                .Create(config =>
                {
                    config.ServiceName = "User";
                    config.ServiceDisplayName = "Cosella User";
                    config.ServiceDescription = "User and Role service for Cosella framework";
                })
                .AddAuthentication()
                .Run();
        }
    }
}