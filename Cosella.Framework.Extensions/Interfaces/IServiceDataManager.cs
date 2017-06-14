namespace Cosella.Framework.Extensions.Interfaces
{
    using Contracts;
    using System.Threading.Tasks;

    public interface IServiceDataManager
    {
        Task<ServiceDescription[]> GetServiceDescriptions(bool includeServiceDescriptor = false);
    }
}