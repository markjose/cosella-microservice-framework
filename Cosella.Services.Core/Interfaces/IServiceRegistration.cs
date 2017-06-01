namespace Cosella.Services.Core.Interfaces
{
    public interface IServiceRegistration
    {
        string InstanceName { get; set; }
        string ApiUrl { get; set; }
    }
}