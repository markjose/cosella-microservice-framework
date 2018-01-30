using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cosella.Framework.Extensions.Gateway
{
    public class ProxyStreamResult : IHttpActionResult
    {
        private readonly Stream _contentStream;
        private readonly string _contentType;

        public ProxyStreamResult(Stream contentStream, string contentType = null)
        {
            _contentStream = contentStream;
            _contentType = contentType;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(_contentStream)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(_contentType);
            response.Content.Headers.Add("max-age", "86400");
            return Task.FromResult(response);
        }
    }
}
