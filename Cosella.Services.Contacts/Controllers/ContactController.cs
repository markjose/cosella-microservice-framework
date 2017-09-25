namespace Cosella.Services.Contacts.Controllers
{
    using Cosella.Framework.Core.Logging;
    using Framework.Core.Attributes;
    using Framework.Core.Controllers;
    using System.Web.Http;

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