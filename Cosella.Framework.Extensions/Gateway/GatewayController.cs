using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Core.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cosella.Framework.Extensions.Gateway
{
    [ControllerDependsOn(typeof(IServiceManager))]
    [RoutePrefix("gateway")]
    public class GatewayController : SystemRestApiController
    {
        private ILogger _log;
        private IServiceManager _serviceManager;

        public GatewayController(ILogger log, IServiceManager serviceManager)
        {
            _log = log;
            _serviceManager = serviceManager;
        }

        [Route("services")]
        [HttpGet]
        public async Task<IHttpActionResult> List([FromUri] bool includeDescriptors)
        {
            try
            {
                return Ok(await _serviceManager.GetServiceDescriptions(includeDescriptors));
            }
            catch (Exception ex)
            {
                return Content(
                    HttpStatusCode.ServiceUnavailable,
                    $"Service unavailable: {ex.InnerException.Message}");
            }
        }

        [Route("services/proxy")]
        [HttpPost]
        public async Task<IHttpActionResult> Proxy([FromBody] ServiceProxyRequest request)
        {
            try
            {
                var response = await _serviceManager.ProxyRequest(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Content(
                    HttpStatusCode.ServiceUnavailable,
                    $"Service unavailable: {ex.Message}");
            }
        }

        [Route("{*resource}")]
        [HttpGet]
        public IHttpActionResult AppResource(string resource)
        {
            return new EmbeddedWebResource(resource);
        }
    }
}