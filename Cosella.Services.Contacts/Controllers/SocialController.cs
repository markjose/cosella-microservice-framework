namespace Cosella.Services.Contacts.Controllers
{
    using Framework.Core.Attributes;
    using Framework.Core.Controllers;
    using log4net;
    using System.Web.Http;

    [RoutePrefix("social")]
    public class SocialController : RestApiController
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