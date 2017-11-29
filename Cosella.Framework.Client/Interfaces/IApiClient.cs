using Cosella.Framework.Client.ApiClient;
using System.Threading.Tasks;

namespace Cosella.Framework.Client.Interfaces
{
    public interface IApiClient
    {
        Task<ApiClientResponse<T>> Get<T>(string uri);

        Task<ApiClientResponse<T>> Delete<T>(string uri);

        Task<ApiClientResponse<T>> Post<T>(string uri, object data);

        Task<ApiClientResponse<T>> Put<T>(string uri, object data);
    }
}