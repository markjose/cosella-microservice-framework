using System;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    public class HostedApplication
    {
        public Guid AppId { get; } = Guid.NewGuid();
        public HostedApplicationMeta Meta { get; } = new HostedApplicationMeta();

        public HostedApplication(HostedApplicationMeta meta)
        {
            Meta = meta;
        }
    }
}