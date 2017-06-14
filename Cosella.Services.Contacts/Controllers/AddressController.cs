namespace Cosella.Services.Contacts.Controllers
{
    using Framework.Core.Attributes;
    using Framework.Core.Logging;
    using System.Web.Http;

    [RoutePrefix("api/v1/addresses")]
    public class AddressController : ApiController
    {
        private ILog _log;

        public AddressController(ILog log)
        {
            _log = log;
        }

        [Route("")]
        [HttpGet]
        [Roles(new[] { "contacts:address:read" })]
        public IHttpActionResult GetAll()
        {
            return Ok();
        }
    }
}