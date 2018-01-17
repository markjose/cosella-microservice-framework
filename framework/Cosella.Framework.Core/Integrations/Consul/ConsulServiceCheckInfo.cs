using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cosella.Framework.Core.Integrations.Consul
{
    public class ConsulServiceHealth
    {
        [JsonProperty("CheckID")]
        public string CheckId { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }

        [JsonProperty("Notes")]
        public string Notes { get; set; }

        [JsonProperty("Output")]
        public string Output { get; set; }

        [JsonProperty("ServiceID")]
        public string ServiceId { get; set; }

        [JsonProperty("ServiceName")]
        public string ServiceName { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("CreateIndex")]
        public int CreateIndex { get; set; }

        [JsonProperty("ModifyIndex")]
        public int ModifyIndex { get; set; }
    }
}