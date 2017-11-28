using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Logging;
using Cosella.Framework.Extensions.Authentication;
using System.Web.Http;

namespace Cosella.Services.Contacts.Controllers
{
    [RoutePrefix("phones")]
    public class TelephoneController : RestApiController
    {
        private ILogger _log;

        public TelephoneController(ILogger log)
        {
            _log = log;
        }

        [Route("")]
        [HttpGet]
        [Roles(new[] { "contacts:phone:read" })]
        public IHttpActionResult GetAll()
        {
            return Ok();
        }
    }
}