using Cosella.Framework.Contracts;
using Cosella.Framework.Core.Hosting;
using log4net;
using System.Net;
using System.Web.Http;

namespace Cosella.Framework.Core.Controllers.Status
{
    [RoutePrefix("status")]
    public class StatusController : SystemRestApiController
    {
        private ILog _log;
        private HostedServiceConfiguration _config;

        public StatusController(ILog log, HostedServiceConfiguration config)
        {
            _log = log;
            _config = config;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetStatus([FromUri] string instanceId = "")
        {
            if (string.IsNullOrWhiteSpace(instanceId) || instanceId == _config.ServiceInstanceName)
            {
                return Ok(MicroServiceStatus.Latest);
            }
            return Content(HttpStatusCode.Unauthorized, "Invalid instance ID for this service, you should deregister any health checks.");
        }
    }
}