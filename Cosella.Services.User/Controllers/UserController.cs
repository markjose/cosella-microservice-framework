namespace Cosella.Services.User.Controllers
{
    using Cosella.Framework.Core.Authentication;
    using Cosella.Framework.Core.Logging;
    using Framework.Contracts;
    using Framework.Core.Configuration;
    using Framework.Core.Controllers;
    using System.Net;
    using System.Web.Http;

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

        [Route("authenticate")]
        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] AuthenticationRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return Content(HttpStatusCode.BadRequest, "Invalid username or password in data body");
            }

            if (request.Username == "test" &&
                request.Password == "test")
            {
                return Ok(new AuthenticationResult()
                {
                    IsAuthorised = true,
                    Name = "Test User",
                    Roles = new string[] { "super:users:read" },
                    Username = request.Username
                });
            }

            return Unauthorized();
        }
    }
}