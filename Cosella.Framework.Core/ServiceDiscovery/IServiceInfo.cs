using System.Collections.Generic;

namespace Cosella.Framework.Core.ServiceDiscovery
{
    public interface IServiceInfo
    {
        string ServiceName { get; set; }
        List<IServiceInstanceInfo> Instances { get; }
    }
}