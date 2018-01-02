using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cosella.Framework.Extensions.Gateway
{
    internal class EmbeddedWebResource : IHttpActionResult
    {
        private string _resource = "";

        private readonly Dictionary<string, string> _mimeFromExtensionMap = new Dictionary<string, string>
        {
            { "js", "application/javascript" },
            { "css", "text/css" },
            { "htm", "text/html" },
            { "html", "text/html" },
            { "svg", "image/svg+xml" },
        };

        public EmbeddedWebResource(string resource)
        {
            _resource = string.IsNullOrWhiteSpace(resource) ? "index.html" : resource.Replace("/",".");
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{assembly.GetName().Name}.Gateway.Console.{_resource}";

            var mimeType = MimeTypeFromResourceName(_resource);
            if (mimeType == null) mimeType = "text/plain";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.ReasonPhrase = $"The resource '{_resource}' was not found";
                    return Task.FromResult(response);
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    response.Content = new StringContent(reader.ReadToEnd());
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                }
            }

            return Task.FromResult(response);
        }

        private string MimeTypeFromResourceName(string resource)
        {
            var extension = resource.Split('.').LastOrDefault();

            return _mimeFromExtensionMap.ContainsKey(extension)
                ? _mimeFromExtensionMap[extension]
                : "text/plain";
        }
    }
}
