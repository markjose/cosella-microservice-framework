using Cosella.Framework.Core.Logging;
using Microsoft.Owin;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cosella.Framework.Core.VersionTracking
{
    public class RestApiVersionMiddleware : OwinMiddleware
    {
        private ILogger _log;
        private string _serviceName;

        public const string ApiVersionFieldName = "api_version";
        public const string ApiFromUriRegex = @"\/api\/v([a-zA-Z0-9]+)";

        public RestApiVersionMiddleware(OwinMiddleware next, ILogger log, string serviceName)
            : base(next)
        {
            _log = log;
            _serviceName = serviceName;
        }

        public async override Task Invoke(IOwinContext context)
        {
            var apiFromUriRegex = new Regex(ApiFromUriRegex, RegexOptions.IgnoreCase);

            var apiVersionFromHeader = context.Request.Headers[ApiVersionFieldName];
            var apiVersionfromQuery = context.Request.Query[ApiVersionFieldName];

            var endpoint = context.Request.Uri.LocalPath;
            var match = apiFromUriRegex.Match(endpoint);
            var apiVersionFromUri = match.Success ? match.Groups[1].ToString() : null;

            var version = string.IsNullOrWhiteSpace(apiVersionFromUri)
                ? (string.IsNullOrWhiteSpace(apiVersionFromHeader)
                    ? apiVersionfromQuery
                    : apiVersionFromHeader)
                : apiVersionFromUri;

            if (version != null)
            {
                _log.Debug($"Service={_serviceName}, Endpoint={endpoint}, ApiVersion={version}");
            }

            await Next.Invoke(context);
        }
    }
}