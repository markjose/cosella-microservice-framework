using System.Net.Http;

namespace Cosella.Framework.Extensions.Gateway
{
    public class ServiceProxyRequest
    {
        public string ServiceName { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string QueryString { get; set; }
        public object Body { get; set; }
    }
}