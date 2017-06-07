namespace Cosella.Services.Core.ServiceDiscovery
{
    public class ServiceInstanceInfo : IServiceInstanceInfo
    {
        public string ServiceName { get; set; }
        public string InstanceName { get; set; }
        public string NodeId { get; set; }
        public string Health { get; set; }
        public int Version { get; set; }
        public string StatusUri { get; set; }
        public string MetadataUri { get; set; }
        public string ApiUri { get; set; }
    }
}