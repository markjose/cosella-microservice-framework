namespace Cosella.Services.Gateway.Interfaces
{
    using Cosella.Contracts;
    using System.Threading.Tasks;

    public interface IServiceDataManager
    {
        Task<ServiceDescription[]> GetServiceDescriptions(bool includeServiceDescriptor = false);
    }
}