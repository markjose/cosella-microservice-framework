using System.Collections.Generic;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    public class ApplicationHostConfiguration
    {
        public bool EnableApplicationManagerApi { get; set; } = false;
        public List<HostedApplicationConfiguration> Applications { get; } = new List<HostedApplicationConfiguration>();
    }
}