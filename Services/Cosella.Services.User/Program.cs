namespace Cosella.Services.User
{
    using Core;

    internal class Program
    {
        private static int Main(string[] args)
        {
            return MicroService.Run(config =>
            {
                config.ServiceName = "User";
                config.ServiceDisplayName = "Cosella User";
                config.ServiceDescription = "User and Role service for Cosella framework";

                config.RestApiPort = 5002;
            });
        }
    }
}