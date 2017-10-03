namespace Cosella.Services.Contacts.Controllers
{
    using Cosella.Framework.Core.Authentication;
    using Cosella.Framework.Core.Logging;
    using Framework.Core.Controllers;
    using System.Web.Http;

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
        [Roles(new[] { "contacts:social:read" })]
        public IHttpActionResult GetAll()
        {
            return Ok();
        }
    }
}