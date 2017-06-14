using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cosella.Framework.Core.Integrations.Consul
{
    internal class ConsulRegistrationRequest
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("Port")]
        public int Port { get; set; }

        [JsonProperty("EnableTagOverride")]
        public bool EnableTagOverride { get; set; }

        [JsonProperty("Check")]
        public ConsulHealthCheck Check { get; set; }
    }
}