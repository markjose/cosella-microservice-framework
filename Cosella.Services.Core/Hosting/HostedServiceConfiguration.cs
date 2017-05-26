using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Cosella.Services.Core.Hosting
{
    public class HostedServiceConfiguration
    {
        public string ServiceName { get; set; }
        public string ServiceDisplayName { get; set; }
        public string ServiceInstanceName { get; set; }
        public string ServiceDescription { get; set; }
        public string ConfigurationService { get; set; }

        // Configuration Overrides
        public int RestApiPort { get; set; } = 5000;

        public int RestApiVersion { get; set; } = 1;
    }
}