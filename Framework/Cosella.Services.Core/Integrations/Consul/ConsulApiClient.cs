using Cosella.Services.Core.ApiClient;
using System.Net;

namespace Cosella.Services.Core.Integrations.Consul
{
    public class ConsulApiClient : ApiClientBase
    {
        private readonly WebClient _client;

        public ConsulApiClient()
            : base("http://localhost:8500/v1")
        {
            _client = new WebClient();
            _client.Headers["ContentType"] = "application/json";
            _client.Headers["Accept"] = "application/json";
        }
    }
}