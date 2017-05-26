using log4net;
using System.Web.Http;

namespace Cosella.Services.Core.Controllers
{
    [RoutePrefix("status")]
    public class StatusController : ApiController
    {
        private ILog _log;

        public StatusController(ILog log)
        {
            _log = log;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetStatus()
        {
            return Ok("OK");
        }
    }
}