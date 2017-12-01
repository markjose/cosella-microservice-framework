using System;
using System.Collections.Generic;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    public class HostedApplicationMeta
    {
        public string Name { get; set; }
        public List<string> Aliases { get; set; } = new List<string>();
        public HostedApplicationTypes ApplicationType { get; set; } = HostedApplicationTypes.None;
        public string ApplicationRoot { get; set; } = "";
    }

}