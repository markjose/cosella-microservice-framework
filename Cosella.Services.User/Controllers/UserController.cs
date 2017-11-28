using Cosella.Framework.Core.Configuration;
using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Logging;
using Cosella.Framework.Extensions.Authentication;
using System.Web.Http;

namespace Cosella.Services.User.Controllers
{
    [RoutePrefix("users")]
    public class UserController : RestApiController
    {
        private ILogger _log;
        private IConfigurator _configurator;

        public UserController(ILogger log, IConfigurator configurator)
        {
            _log = log;
            _configurator = configurator;
        }

        [Route("")]
        [HttpGet]
        [Roles(new[] { "super:user:read" })]
        public IHttpActionResult GetAll()
        {
            return Ok();
        }
    }
}