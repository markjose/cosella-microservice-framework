namespace Cosella.Services.Authenticator
{
    using Core;

    internal class Program
    {
        private static int Main(string[] args)
        {
            return MicroService
                .Create(config =>
                {
                    config.ServiceName = "Authenticator";
                    config.ServiceDisplayName = "Cosella Authenticator";
                    config.ServiceDescription = "Token and App authentication service for Cosella framework";
                })
                .Run();
        }
    }
}