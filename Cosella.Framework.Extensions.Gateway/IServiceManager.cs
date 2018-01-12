using Cosella.Framework.Client.ApiClient;
using Cosella.Framework.Core.Contracts;
using System.Threading.Tasks;

namespace Cosella.Framework.Extensions.Gateway
{
    public interface IServiceManager
    {
        Task<ServiceDescription[]> GetServiceDescriptions(bool includeServiceDescriptor = false);
        Task<ServiceRestResponse<object>> ProxyRequest(ServiceProxyRequest request);
    }
}