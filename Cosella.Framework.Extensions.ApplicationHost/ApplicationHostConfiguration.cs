using System.Collections.Generic;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    public class ApplicationHostConfiguration
    {
        public bool EnableApplicationManagerApi { get; set; } = false;
        public ApplicationList Applications { get; } = new ApplicationList();

        public class ApplicationList : List<HostedApplicationConfiguration>
        {
            public void Add(string name, string primaryAlias, bool isDefault = false)
            {
                Add(new HostedApplicationConfiguration
                {
                    Name = name,
                    Aliases = new [] { primaryAlias },
                    IsDefault = isDefault
                });
            }
        }
    }
}