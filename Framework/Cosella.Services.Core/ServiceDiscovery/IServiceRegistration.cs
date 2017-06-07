namespace Cosella.Services.Core.ServiceDiscovery
{
    public interface IServiceRegistration
    {
        string InstanceName { get; set; }
        string ApiUrl { get; set; }
    }
}