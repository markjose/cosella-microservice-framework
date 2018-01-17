using System.Collections.Generic;

namespace Cosella.Framework.Client.Interfaces
{
    public interface IServiceInfo
    {
        string ServiceName { get; set; }
        List<IServiceInstanceInfo> Instances { get; }
    }
}