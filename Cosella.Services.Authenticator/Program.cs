namespace Cosella.Services.Authenticator
{
    using Core;

    internal class Program
    {
        private static int Main(string[] args)
        {
            return MicroService.Run(config =>
            {
                config.ServiceName = "Authenticator";
                config.ServiceDisplayName = "Cosella Authenticator";
                config.ServiceDescription = "Token, User and Role service for Cosella framework";
            });
        }
    }
}