using System.Collections.Generic;

namespace Cosella.Services.Core.Interfaces
{
    public interface IServiceInfo
    {
        string ServiceName { get; set; }
        List<IServiceInstanceInfo> Instances { get; }
    }
}