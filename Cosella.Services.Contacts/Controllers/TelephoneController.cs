namespace Cosella.Services.Contacts.Controllers
{
    using Cosella.Framework.Core.Logging;
    using Framework.Core.Attributes;
    using Framework.Core.Controllers;
    using System.Web.Http;

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