namespace Cosella.Services.User
{
    using Framework.Core;

    internal class Program
    {
        private static int Main(string[] args)
        {
            return MicroService
                .Create(test =>
                {
                    test.ServiceName = "User";
                    test.ServiceDisplayName = "Cosella User";
                    test.ServiceDescription = "User and Role service for Cosella framework";
                })
                .Run();
        }
    }
}