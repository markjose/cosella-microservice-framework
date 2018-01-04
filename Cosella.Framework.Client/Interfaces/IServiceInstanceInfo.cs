namespace Cosella.Framework.Client.Interfaces
{
    public interface IServiceInstanceInfo
    {
        string ServiceName { get; set; }
        string InstanceName { get; set; }
        string NodeId { get; set; }
        string Health { get; set; }
        int Version { get; set; }
        string BaseUri { get; set; }
        string StatusUri { get; set; }
        string ApiUri { get; set; }
        string MetadataUri { get; set; }
    }
}