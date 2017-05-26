namespace Cosella.Services.Authenticator.Controllers
{
    using Contracts;
    using log4net;
    using System.Web.Http;

    [RoutePrefix("api")]
    public class TokenController : ApiController
    {
        private ILog _log;

        public TokenController(ILog log)
        {
            _log = log;
        }

        [Route("token")]
        [HttpPost]
        public IHttpActionResult GetToken([FromBody] TokenRequest request)
        {
            return Ok($"TOKEN {request.Username}");
        }
    }
}