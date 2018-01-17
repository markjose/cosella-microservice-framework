using Ninject;
using Ninject.Modules;
using Owin;
using System;
using System.Collections.Generic;

namespace Cosella.Framework.Core.Hosting
{
    public class HostedServiceConfiguration
    {
        public List<INinjectModule> Modules { get; } = new List<INinjectModule>();
        public List<Func<IAppBuilder, IKernel, IAppBuilder>> Middleware { get; } = new List<Func<IAppBuilder, IKernel, IAppBuilder>>();

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