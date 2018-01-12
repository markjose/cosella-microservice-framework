using Cosella.Framework.Core.Hosting;

namespace Cosella.Framework.Core
{
    public interface IMicroServiceContainer
    {
        HostedServiceConfiguration Configuration { get; }

        void Init(HostedServiceConfiguration configuration);
        int Run();
    }
}
