using Cosella.Framework.Core;

namespace Cosella.Services.MinimumConfig
{
    class Program
    {
        static int Main(string[] args)
        {
            return MicroService
                .Configure()
                .Run();
        }
    }
}
