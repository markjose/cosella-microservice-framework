using System.Collections.Generic;

namespace Cosella.Services.Core.ServiceDiscovery
{
    public class ServiceInfo : IServiceInfo
    {
        public List<IServiceInstanceInfo> Instances { get; } = new List<IServiceInstanceInfo>();
        public string ServiceName { get; set; }
    }
}