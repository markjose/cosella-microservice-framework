namespace Cosella.Framework.Core
{
    using ApiClient;
    using ServiceDiscovery;

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

        public static ServiceRestApiClient Create(string serviceName, IServiceDiscovery discovery)
        {
            var client = new ServiceRestApiClient(serviceName, discovery);
            return client.Init() ? client : null;
        }
    }
}