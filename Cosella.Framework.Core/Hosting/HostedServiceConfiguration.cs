namespace Cosella.Framework.Core.Hosting
{
    using Ninject.Modules;
    using System.Collections.Generic;

    public class HostedServiceConfiguration
    {
        public List<INinjectModule> Modules { get; } = new List<INinjectModule>();

        public string ServiceName { get; set; }
        public string ServiceDisplayName { get; set; }
        public string ServiceInstanceName { get; set; }
        public string ServiceDescription { get; set; }
        public string ConfigurationService { get; set; }

        // Configuration Overrides

        public int RestApiPort { get; set; } = 0;
        public int RestApiVersion { get; set; } = 1;
        public string RestApiHostname { get; set; } = "localhost";

        // Service discovery

        public bool DisableRegistration { get; set; } = false;
        public bool DisableServiceDiscovery { get; set; } = false;
    }
}