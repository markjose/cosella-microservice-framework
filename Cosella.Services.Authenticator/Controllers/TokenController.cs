namespace Cosella.Services.Authenticator.Controllers
{
    using Contracts;
    using Core.Interfaces;
    using JWT;
    using JWT.Algorithms;
    using JWT.Serializers;
    using log4net;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Http;

    [RoutePrefix("api/v1/token")]
    public class TokenController : ApiController
    {
        private ILog _log;
        private IConfigurator _configurator;
        private IJsonSerializer _serializer;
        private IBase64UrlEncoder _urlEncoder;
        private IJwtAlgorithm _algorithm;
        private IDateTimeProvider _provider;
        private IJwtValidator _validator;

        public TokenController(ILog log, IConfigurator configurator)
        {
            _log = log;
            _configurator = configurator;

            _serializer = new JsonNetSerializer();
            _urlEncoder = new JwtBase64UrlEncoder();
            _algorithm = new HMACSHA256Algorithm();
            _provider = new UtcDateTimeProvider();
            _validator = new JwtValidator(_serializer, _provider);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult GetToken([FromBody] TokenRequest request)
        {
            var payload = new Dictionary<string, object>
            {
                { "claim1", 0 },
                { "claim2", "claim2-value" }
            };
            var secret = _configurator.GetString("Jwt", "Secret");

            IJwtEncoder encoder = new JwtEncoder(_algorithm, _serializer, _urlEncoder);

            return Ok(encoder.Encode(payload, secret));
        }

        [Route("verify/{token}")]
        [HttpGet]
        public IHttpActionResult VerifyToken(string token)
        {
            var secret = _configurator.GetString("Jwt", "Secret");
            try
            {
                IJwtDecoder decoder = new JwtDecoder(_serializer, _validator, _urlEncoder);

                var json = decoder.Decode(token, secret, verify: true);
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