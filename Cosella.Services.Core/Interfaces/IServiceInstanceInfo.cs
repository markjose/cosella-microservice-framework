namespace Cosella.Services.Core.Interfaces
{
    public interface IServiceInstanceInfo
    {
        string InstanceName { get; set; }
        string NodeId { get; set; }
        int Version { get; set; }
        string StatusUri { get; set; }
        string MetadataUri { get; set; }
    }
}