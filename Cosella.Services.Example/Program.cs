using Cosella.Framework.Core;
using Cosella.Framework.Extensions.ServiceFabric;

namespace Cosella.Services.Example
{
    class Program
    {
        static int Main(string[] args)
        {
            return MicroService
                .Configure(config =>
                {
                    config.ServiceName = "Cosella.Example";
                    config.ServiceDisplayName = "Cosella Example Service";
                })
                .Run();
        }
    }
}
