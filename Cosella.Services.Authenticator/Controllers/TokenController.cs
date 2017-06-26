namespace Cosella.Services.Authenticator.Controllers
{
    using Framework.Contracts;
    using Framework.Core;
    using Framework.Core.ApiClient;
    using Framework.Core.Configuration;
    using Framework.Core.ServiceDiscovery;

    using JWT;
    using JWT.Algorithms;
    using JWT.Serializers;
    using log4net;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Http;

    [RoutePrefix("token")]
    public class TokenController : ApiController
    {
        private ILog _log;
        private IServiceDiscovery _discovery;
        private IConfigurator _configurator;
        private IJsonSerializer _serializer;
        private IBase64UrlEncoder _urlEncoder;
        private IJwtAlgorithm _algorithm;
        private IDateTimeProvider _provider;
        private IJwtValidator _validator;

        public TokenController(ILog log, IConfigurator configurator, IServiceDiscovery discovery)
        {
            _log = log;
            _discovery = discovery;
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
            var userService = ServiceRestApiClient.Create("User", _discovery);
            if (userService == null)
            {
                return Content(HttpStatusCode.ServiceUnavailable, $"The User service could not be found.");
            }

            var auth = userService.Post<AuthenticationResult>("users/authenticate", new AuthenticationRequest()
            {
                Username = request.Username,
                Password = request.Password
            }).Result;

            if (auth.Status == ApiClientResponseStatus.Ok)
            {
                if (auth.Payload.IsAuthorised)
                {
                    var payload = new Dictionary<string, object>
                    {
                        { "username", auth.Payload.Username },
                        { "name", auth.Payload.Name },
                        { "roles", auth.Payload.Roles }
                    };
                    var secret = _configurator.GetString("Jwt", "Secret");
                    IJwtEncoder encoder = new JwtEncoder(_algorithm, _serializer, _urlEncoder);
                    return Ok(encoder.Encode(payload, secret));
                }
                return Unauthorized();
            }
            return Content(HttpStatusCode.ServiceUnavailable, auth.Exception);
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