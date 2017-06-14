namespace Cosella.Services.Contacts.Controllers
{
    using Framework.Core.Attributes;
    using Framework.Core.Logging;
    using System.Web.Http;

    [RoutePrefix("api/v1/contacts")]
    public class ContactController : ApiController
    {
        private ILog _log;

        public ContactController(ILog log)
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