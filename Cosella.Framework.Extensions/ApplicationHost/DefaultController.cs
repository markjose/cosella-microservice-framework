using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using Ninject;
using System.Web.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using Cosella.Framework.Core.Logging;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    [ControllerDependsOn(typeof(IApplicationManager))]
    [RoutePrefix("")]
    public class _DefaultController : SystemRestApiController
    {
        private readonly ILogger _log;
        private readonly IApplicationManager _applicationManager;

        public _DefaultController(IKernel kernel)
        {
            _log = kernel.Get<ILogger>();
            _applicationManager = kernel.Get<IApplicationManager>();
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult Default([FromUri] string tenant = null)
        {
            return GetRootContent(tenant);
        }

        [Route("{tenant}/")]
        [HttpGet]
        public IHttpActionResult TenantDefault(string tenant)
        {
            return GetRootContent(tenant);
        }

        private IHttpActionResult GetRootContent(string aliasOrAppId)
        {
            HostedApplication app = null;
            try
            {
                app = _applicationManager.Get(aliasOrAppId);
            }
            catch (ApplicationHostException)
            {
                try
                {
                    app = _applicationManager.FromAlias(aliasOrAppId);
                }
                catch (ApplicationHostException)
                {
                    app = _applicationManager.DefaultApp;
                }
            }

            if (app == null)
            {
                return SendResource(ReadTextResource("NoTenantRoot.html"), "text/html");
            }

            var appRoot = GetApplicationRoot(app.Meta);

            var resource = string.IsNullOrWhiteSpace(appRoot)
                ? ReadTextResource("TenantRoot.html")
                : ReadTextContent(appRoot);

            resource = resource.Replace("[[TenantName]]", app.Meta.Name);
            resource = resource.Replace("[[TenantAppId]]", app.AppId.ToString());

            return SendResource(resource, "text/html");
        }

        private string GetApplicationRoot(HostedApplicationMeta meta)
        {
            if (string.IsNullOrWhiteSpace(meta.ApplicationRoot)) return "";
            if (meta.ApplicationType == HostedApplicationTypes.React) return $"{meta.ApplicationRoot}/index.html";
            return "";
        }

        private IHttpActionResult SendResource(string resource, string mimeType)
        {
            try
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(resource) };
                var type = new MediaTypeHeaderValue(mimeType);
                result.Content.Headers.ContentType = type;
                return ResponseMessage(result);
            }
            catch (ApplicationHostException ex)
            {
                return ex.NotFound
                    ? (IHttpActionResult)NotFound()
                    : BadRequest(ex.Message);
            }
        }

        private string ReadTextResource(string resourceName)
        {
            var assembly = GetType().Assembly;
            var resourcePath = $"{assembly.GetName().Name}.ApplicationHost.Content.{resourceName}";

            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        private string ReadTextContent(string appRoot)
        {
            var uri = new UriBuilder(Assembly.GetEntryAssembly().CodeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var file = Path.Combine(Path.GetDirectoryName(path), appRoot);

            try
            {
                return File.ReadAllText(file);
            }
            catch (Exception ex)
            {
                _log.Fatal($"Failed to load content from {file}", ex);
                return "";
            }
        }
    }
}
