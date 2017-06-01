using System;
using System.Threading.Tasks;
using Cosella.Services.Core.Interfaces;
using System.Net;
using Newtonsoft.Json;

namespace Cosella.Services.Core.Communications
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