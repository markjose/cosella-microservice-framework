using Cosella.Services.Core.Communications;
using System.Threading.Tasks;

namespace Cosella.Services.Core.Interfaces
{
    public interface IApiClient
    {
        Task<ApiClientResponse<T>> Get<T>(string uri);

        Task<ApiClientResponse<T>> Delete<T>(string uri);

        Task<ApiClientResponse<T>> Post<T>(string uri, object data);

        Task<ApiClientResponse<T>> Put<T>(string uri, object data);
    }
}