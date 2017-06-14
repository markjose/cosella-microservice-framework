namespace Cosella.Framework.Core.ServiceDiscovery
{
    public interface IServiceRegistration
    {
        string InstanceName { get; set; }
        string ApiUrl { get; set; }
    }
}