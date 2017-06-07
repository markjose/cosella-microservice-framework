using System.Collections.Generic;

namespace Cosella.Services.Core.ServiceDiscovery
{
    public interface IServiceInfo
    {
        string ServiceName { get; set; }
        List<IServiceInstanceInfo> Instances { get; }
    }
}