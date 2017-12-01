using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Extensions.Authentication;
using Ninject;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    [ControllerDependsOn(typeof(IApplicationManagerOverRest))]
    [RoutePrefix("applications")]
    public class ApplicationHostController : RestApiController
    {
        private readonly IApplicationManager _applicationManager;

        public ApplicationHostController(IKernel kernel)
        {
            _applicationManager = kernel.Get<IApplicationManager>();
        }

        [Route("")]
        [HttpGet]
        [Authentication("ApplicationHost")]
        public IHttpActionResult List()
        {
            return Ok(_applicationManager.List());
        }

        [Route("")]
        [HttpPost]
        [Authentication("ApplicationHost:Admin")]
        public IHttpActionResult AddApp([FromBody] HostedApplicationMeta appRequest)
        {
            try
            {
                return Ok(_applicationManager.Add(appRequest));
            }
            catch (ApplicationHostException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("")]
        [HttpDelete]
        [Authentication("ApplicationHost:Admin")]
        public IHttpActionResult RemoveApp(string appId)
        {
            try
            {
                return Ok(_applicationManager.Remove(appId));
            }
            catch (ApplicationHostException ex)
            {
                return ex.NotFound
                    ? (IHttpActionResult)NotFound()
                    : BadRequest(ex.Message);
            }
        }

        [Route("{appId}")]
        [HttpGet]
        [Authentication("ApplicationHost:Admin")]
        public IHttpActionResult GetApp(string appId)
        {
            try
            {
                return Ok(_applicationManager.Get(appId));
            }
            catch (ApplicationHostException ex)
            {
                return ex.NotFound
                    ? (IHttpActionResult)NotFound()
                    : BadRequest(ex.Message);
            }
        }

        [Route("{appId}/resources/{resourceId}")]
        [HttpGet]
        public IHttpActionResult GetAppResource(string appId, string resourceId)
        {
            try
            {
                var resource = _applicationManager.GetResource(appId, resourceId);
                try
                {
                    var content = new ByteArrayContent(File.ReadAllBytes(resource.DataPath));
                    var result = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
                    var disposition = new ContentDispositionHeaderValue("attachment") { FileName = resource.Filename };
                    var type = new MediaTypeHeaderValue("application/octet-stream");

                    result.Content.Headers.ContentDisposition = disposition;
                    result.Content.Headers.ContentType = type;

                    return ResponseMessage(result);
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            catch (ApplicationHostException ex)
            {
                return ex.NotFound
                    ? (IHttpActionResult)NotFound()
                    : BadRequest(ex.Message);
            }
        }
    }
}
