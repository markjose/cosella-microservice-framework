using Newtonsoft.Json;

namespace Cosella.Framework.Core.Integrations.Consul
{
    public class ConsulHealthCheck
    {
        [JsonProperty("DeregisterCriticalServiceAfter")]
        public string DeregisterCriticalServiceAfter { get; set; }

        [JsonProperty("Script")]
        public string Script { get; set; }

        [JsonProperty("HTTP")]
        public string Http { get; set; }

        [JsonProperty("Interval")]
        public string Interval { get; set; }

        [JsonProperty("TTL")]
        public string Ttl { get; set; }
    }
}