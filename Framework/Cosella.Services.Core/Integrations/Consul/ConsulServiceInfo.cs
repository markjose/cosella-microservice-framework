using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cosella.Services.Core.Integrations.Consul
{
    public class ConsulServiceInfo
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("Service")]
        public string Service { get; set; }

        [JsonProperty("Tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("Port")]
        public int Port { get; set; }

        [JsonProperty("EnableTagOverride")]
        public bool EnableTagOverride { get; set; }

        [JsonProperty("CreateIndex")]
        public int CreateIndex { get; set; }

        [JsonProperty("ModifyIndex")]
        public int ModifyIndex { get; set; }
    }
}