using Cosella.Framework.Core;

namespace Cosella.Services.Example
{
    class Program
    {
        static int Main(string[] args)
        {
            return MicroService
                .Create(config =>
                {
                    config.ServiceName = "Cosella.Example";
                    config.ServiceDisplayName = "Cosella Example Service";
                })
                .Run();
        }
    }
}
