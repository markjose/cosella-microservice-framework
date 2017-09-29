namespace Cosella.Framework.Extensions.Controllers
{
    using Cosella.Framework.Core.Logging;
    using Cosella.Framework.Extensions.Interfaces;
    using Framework.Contracts;
    using Framework.Core.Controllers;

    using JWT;
    using JWT.Algorithms;
    using JWT.Serializers;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Web.Http;

    [RoutePrefix("auth")]
    public class AuthController : RestApiController
    {
        private readonly ILogger _log;
        private readonly ITokenManager _tokenManager;

        private IJsonSerializer _serializer;
        private IBase64UrlEncoder _urlEncoder;
        private IJwtAlgorithm _algorithm;
        private IDateTimeProvider _provider;
        private IJwtValidator _validator;

        public AuthController(ILogger log, ITokenManager tokenManager)
        {
            _log = log;
            _tokenManager = tokenManager;

            _serializer = new JsonNetSerializer();
            _urlEncoder = new JwtBase64UrlEncoder();
            _algorithm = new HMACSHA256Algorithm();
            _provider = new UtcDateTimeProvider();
            _validator = new JwtValidator(_serializer, _provider);
        }

        [Route("token")]
        [HttpPost]
        public IHttpActionResult GetToken([FromBody] TokenRequest request)
        {
            if (request == null) return BadRequest();

            var token = _tokenManager.Create(request.Username, request.Password);
            return token == null ? (IHttpActionResult)Unauthorized() : Ok(token);
        }

        [Route("verify/{token}")]
        [HttpGet]
        public IHttpActionResult VerifyToken(string token)
        {
            try
            {
                _tokenManager.Verify(token);
            }
            catch (ArgumentException)
            {
                return BadRequest($"Token was invalid, corrupted or malformed");
            }
            catch (JsonReaderException)
            {
                return BadRequest($"Token was invalid, corrupted or malformed");
            }
            catch (TokenExpiredException)
            {
                return Content(HttpStatusCode.Unauthorized, $"Token has expired");
            }
            catch (SignatureVerificationException)
            {
                return Content(HttpStatusCode.Unauthorized, $"Token has invalid signature");
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