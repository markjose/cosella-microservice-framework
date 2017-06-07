using Cosella.Services.Core.Controllers.Status;
using Cosella.Services.Core.Hosting;
using log4net;
using System.Net;
using System.Web.Http;

namespace Cosella.Services.Core.Controllers
{
    [RoutePrefix("status")]
    public class StatusController : ApiController
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