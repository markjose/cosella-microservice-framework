namespace Cosella.Services.Contacts
{
    using Framework.Core;

    internal class Program
    {
        private static int Main(string[] args)
        {
            return MicroService
                .ConfiguredFor(MicroServiceType.WindowsService, config =>
                {
                    config.ServiceName = "Contacts";
                    config.ServiceDisplayName = "Cosella Contacts";
                    config.ServiceDescription = "Contacts service for Cosella framework";
                })
                .Run();
        }
    }
}