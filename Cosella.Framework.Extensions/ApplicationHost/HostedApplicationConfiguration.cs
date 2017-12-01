namespace Cosella.Framework.Extensions.ApplicationHost
{
    public class HostedApplicationConfiguration
    {
        public string Name { get; set; } = "New App";
        public string[] Aliases { get; set; } = new string[0];
        public bool IsDefault { get; set; } = false;
        public HostedApplicationTypes ApplicationType { get; set; } = HostedApplicationTypes.None;
        public string ApplicationRoot { get; set; } = "";
    }
}