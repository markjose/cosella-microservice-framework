namespace Cosella.Framework.Client.Interfaces
{
    public interface IServiceRegistration
    {
        string InstanceName { get; set; }
        string ApiUrl { get; set; }
    }
}