using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Logging;
using Cosella.Framework.Extensions.Authentication;
using System.Web.Http;

namespace Cosella.Services.Contacts.Controllers
{
    [RoutePrefix("social")]
    public class SocialController : RestApiController
    {
        private ILogger _log;

        public SocialController(ILogger log)
        {
            _log = log;
        }

        [Route("")]
        [HttpGet]
        [Authentication("contacts:social:read")]
        public IHttpActionResult GetAll()
        {
            return Ok();
        }
    }
}