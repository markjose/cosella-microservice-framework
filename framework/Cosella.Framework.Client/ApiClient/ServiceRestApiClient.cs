using Cosella.Framework.Client.Interfaces;
using System.Threading.Tasks;

namespace Cosella.Framework.Client.ApiClient
{
    public class ServiceRestApiClient : ApiClientBase
    {
        private IServiceInstanceInfo _serviceInstance;
        private IServiceDiscovery _discovery;
        private string _serviceName;

        private ServiceRestApiClient(string serviceName, IServiceDiscovery discovery) : base("")
        {
            _serviceName = serviceName;
            _discovery = discovery;
        }

        private async Task<bool> Init()
        {
            _serviceInstance = await _discovery.FindServiceByName(_serviceName);
            if (_serviceInstance != null)
            {
                _baseUrl = _serviceInstance.BaseUri;
                return true;
            }
            return false;
        }

        public static async Task<ServiceRestApiClient> Create(string serviceName, IServiceDiscovery discovery)
        {
            var client = new ServiceRestApiClient(serviceName, discovery);
            return await client.Init() ? client : null;
        }

        public new async Task<ServiceRestResponse<T>> Get<T>(string uri)
        {
            return new ServiceRestResponse<T>(await base.Get<T>(uri), _serviceInstance);
        }

        public new async Task<ServiceRestResponse<T>> Delete<T>(string uri)
        {
            return new ServiceRestResponse<T>(await base.Delete<T>(uri), _serviceInstance);
        }

        public new async Task<ServiceRestResponse<T>> Post<T>(string uri, object data)
        {
            return new ServiceRestResponse<T>(await base.Post<T>(uri, data), _serviceInstance);
        }

        public new async Task<ServiceRestResponse<T>> Put<T>(string uri, object data)
        {
            return new ServiceRestResponse<T>(await base.Put<T>(uri, data), _serviceInstance);
        }

        public new async Task<ServiceRestResponse<T>> Patch<T>(string uri, object data)
        {
            return new ServiceRestResponse<T>(await base.Patch<T>(uri, data), _serviceInstance);
        }
    }
}