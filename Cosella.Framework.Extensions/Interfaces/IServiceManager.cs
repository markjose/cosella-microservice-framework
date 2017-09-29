namespace Cosella.Framework.Extensions.Interfaces
{
    using Contracts;
    using System.Threading.Tasks;

    public interface IServiceManager
    {
        Task<ServiceDescription[]> GetServiceDescriptions(bool includeServiceDescriptor = false);
    }
}