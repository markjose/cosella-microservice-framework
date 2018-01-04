using Cosella.Framework.Client.Interfaces;
using System.Threading.Tasks;

namespace Cosella.Framework.Client.ApiClient
{
    public class ServiceRestApiClient : ApiClientBase
    {
        private IServiceInstanceInfo _service;
        private IServiceDiscovery _discovery;
        private string _serviceName;

        private ServiceRestApiClient(string serviceName, IServiceDiscovery discovery) : base("")
        {
            _serviceName = serviceName;
            _discovery = discovery;
        }

        private async Task<bool> Init()
        {
            _service = await _discovery.FindServiceByName(_serviceName);
            if (_service != null)
            {
                _baseUrl = _service.BaseUri;
                return true;
            }
            return false;
        }

        public static async Task<ServiceRestApiClient> Create(string serviceName, IServiceDiscovery discovery)
        {
            var client = new ServiceRestApiClient(serviceName, discovery);
            return await client.Init() ? client : null;
        }
    }
}