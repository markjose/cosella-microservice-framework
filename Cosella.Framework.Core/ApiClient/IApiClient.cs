using Cosella.Framework.Core.ApiClient;
using System.Threading.Tasks;

namespace Cosella.Framework.Core.Configuration
{
    public interface IApiClient
    {
        Task<ApiClientResponse<T>> Get<T>(string uri);

        Task<ApiClientResponse<T>> Delete<T>(string uri);

        Task<ApiClientResponse<T>> Post<T>(string uri, object data);

        Task<ApiClientResponse<T>> Put<T>(string uri, object data);
    }
}