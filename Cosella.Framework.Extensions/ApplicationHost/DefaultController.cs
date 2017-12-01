using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using Ninject;
using System.Web.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    [ControllerDependsOn(typeof(IApplicationManager))]
    [RoutePrefix("")]
    public class _DefaultController : SystemRestApiController
    {
        private readonly IApplicationManager _applicationManager;

        public _DefaultController(IKernel kernel)
        {
            _applicationManager = kernel.Get<IApplicationManager>();
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult Default([FromUri] string tenant = null)
        {
            return GetRootContent(tenant);
        }

        [Route("{tenant}")]
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

            var resource = ReadTextResource("TenantRoot.html");
            resource = resource.Replace("[[TenantName]]", app.Meta.Name);
            resource = resource.Replace("[[TenantAppId]]", app.AppId.ToString());
            return SendResource(resource, "text/html");
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
    }
}
