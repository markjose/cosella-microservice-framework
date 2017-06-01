using System.Collections.Generic;
using Cosella.Services.Core.Interfaces;

namespace Cosella.Services.Core.Models
{
    public class ServiceInfo : IServiceInfo
    {
        public List<IServiceInstanceInfo> Instances { get; } = new List<IServiceInstanceInfo>();
        public string ServiceName { get; set; }
    }
}