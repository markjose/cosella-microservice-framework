using Cosella.Services.Core.ApiClient;
using Cosella.Services.Core.ServiceDiscovery;

namespace Cosella.Services.Core
{
    public class CosellaApiClient : ApiClientBase
    {
        private IServiceInstanceInfo _service;
        private IServiceDiscovery _discovery;
        private string _serviceName;

        private CosellaApiClient(string serviceName, IServiceDiscovery discovery) : base("")
        {
            _serviceName = serviceName;
            _discovery = discovery;
        }

        private bool Init()
        {
            _service = _discovery.FindServiceByName(_serviceName).Result;
            if (_service != null)
            {
                _baseUrl = _service.ApiUri;
                return true;
            }
            return false;
        }

        public static CosellaApiClient Create(string serviceName, IServiceDiscovery discovery)
        {
            var client = new CosellaApiClient(serviceName, discovery);
            return client.Init() ? client : null;
        }
    }
}