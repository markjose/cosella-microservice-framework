namespace Cosella.Framework.Extensions.Controllers
{
    using Cosella.Framework.Core.Authentication;
    using Cosella.Framework.Core.Logging;
    using Framework.Contracts;
    using Framework.Core.Controllers;

    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Web.Http;

    [RoutePrefix("auth")]
    public class AuthController : RestApiController
    {
        private readonly ILogger _log;
        private readonly ITokenManager _tokenManager;

        public AuthController(ILogger log, ITokenManager tokenManager)
        {
            _log = log;
            _tokenManager = tokenManager;
        }

        [Route("token")]
        [HttpPost]
        public IHttpActionResult GetToken([FromBody] TokenRequest request)
        {
            if (request == null) return BadRequest();

            var token = _tokenManager.Create(request.Username, request.Password);
            return token == null
                ? (IHttpActionResult)Unauthorized()
                : Ok(token);
        }

        [Route("verify/{token}")]
        [HttpGet]
        public IHttpActionResult VerifyToken(string token)
        {
            try
            {
                _tokenManager.Verify(token);
            }
            catch (TokenManagerException ex)
            {
                return ex.IsInvalid
                    ? (IHttpActionResult)BadRequest(ex.Message)
                    : Content(HttpStatusCode.Unauthorized, ex.Message);
            }
            return Ok();
        }

        [Route("refresh/{token}")]
        [HttpGet]
        public IHttpActionResult RefreshToken(string token)
        {
            return NotFound();
        }
    }
}