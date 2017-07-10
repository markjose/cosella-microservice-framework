namespace Cosella.Services.Contacts.Controllers
{
    using Framework.Core.Attributes;
    using Framework.Core.Controllers;
    using log4net;
    using System.Web.Http;

    [RoutePrefix("phones")]
    public class TelephoneController : RestApiController
    {
        private ILog _log;

        public TelephoneController(ILog log)
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