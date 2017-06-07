namespace Cosella.Services.Core.ServiceDiscovery
{
    internal class ServiceRegistration : IServiceRegistration
    {
        public string InstanceName { get; set; }
        public string ApiUrl { get; set; }
    }
}