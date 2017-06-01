using Cosella.Services.Core.Interfaces;

namespace Cosella.Services.Core.Communications
{
    public class CosellaApiClient : ApiClientBase
    {
        private IServiceRegistration _service;

        public CosellaApiClient(string serviceName, IServiceDiscovery discovery) : base("")
        {
            _service = discovery.FindServiceByName(serviceName);
            if (_service != null)
            {
                _baseUrl = _service.ApiUrl;
            }
        }
    }
}