using Cosella.Framework.Client.Interfaces;
using System.Collections.Generic;

namespace Cosella.Framework.Core.ServiceDiscovery
{
    public class ServiceInfo : IServiceInfo
    {
        public List<IServiceInstanceInfo> Instances { get; } = new List<IServiceInstanceInfo>();
        public string ServiceName { get; set; }
    }
}