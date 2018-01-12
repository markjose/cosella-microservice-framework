namespace Cosella.Framework.Extensions.Gateway
{
    public class ServiceApiDescriptor
    {
        public string Schema { get; internal set; }
        public string ServiceName { get; internal set; }
        public int Version { get; internal set; }
    }
}