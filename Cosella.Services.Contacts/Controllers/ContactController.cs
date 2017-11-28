using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Logging;
using Cosella.Framework.Extensions.Authentication;
using System.Web.Http;

namespace Cosella.Services.Contacts.Controllers
{
    [RoutePrefix("contacts")]
    public class ContactController : RestApiController
    {
        private ILogger _log;

        public ContactController(ILogger log)
        {
            _log = log;
        }

        [Route("")]
        [HttpGet]
        [Roles(new[] { "contacts:contact:read" })]
        public IHttpActionResult GetAll()
        {
            return Ok();
        }
    }
}