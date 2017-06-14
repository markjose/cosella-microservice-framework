namespace Cosella.Services.Contacts.Controllers
{
    using Framework.Core.Attributes;
    using Framework.Core.Logging;
    using System.Web.Http;

    [RoutePrefix("api/v1/social")]
    public class SocialController : ApiController
    {
        private ILog _log;

        public SocialController(ILog log)
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