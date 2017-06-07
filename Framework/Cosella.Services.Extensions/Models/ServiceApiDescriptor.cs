namespace Cosella.Services.Extensions.Models
{
    public class ServiceApiDescriptor
    {
        public string Schema { get; internal set; }
        public string ServiceName { get; internal set; }
        public int Version { get; internal set; }
    }
}