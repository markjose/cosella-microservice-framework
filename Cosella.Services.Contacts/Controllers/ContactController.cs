namespace Cosella.Services.Contacts.Controllers
{
    using Framework.Core.Attributes;
    using Framework.Core.Controllers;
    using log4net;
    using System.Web.Http;

    [RoutePrefix("contacts")]
    public class ContactController : RestApiController
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