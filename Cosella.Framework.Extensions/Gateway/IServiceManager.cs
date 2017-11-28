using Cosella.Framework.Core.Contracts;
using System.Threading.Tasks;

namespace Cosella.Framework.Extensions.Gateway
{
    public interface IServiceManager
    {
        Task<ServiceDescription[]> GetServiceDescriptions(bool includeServiceDescriptor = false);
    }
}