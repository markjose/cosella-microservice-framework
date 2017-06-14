namespace Cosella.Services.User
{
    using Framework.Core;

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
                .Run();
        }
    }
}