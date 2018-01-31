using Cosella.Framework.Core.Logging;
using Cosella.Framework.Extensions.Authentication.Interfaces;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Security;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    public class DefaultTokenHandler : ITokenHandler
    {
        private const int SecretLength = 48;

        private string _secret;
        private ILogger _log;

        private IJsonSerializer _serializer = new JsonNetSerializer();
        private IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();

        public DefaultTokenHandler(IKernel kernel)
        {
            _log = kernel.Get<ILogger>();

            var config = kernel.Get<AuthenticationConfiguration>();

            _secret = config.SimpleTokenControllerSecret;
            if(string.IsNullOrWhiteSpace(_secret))
            {
                _secret = Membership.GeneratePassword(SecretLength, 0);
            }
        }

        public IEnumerable<AuthenticationTokenSource> TokenSources => new[]
        {
            new AuthenticationTokenSource
            {
                Type = AuthenticationTokenSourceType.Header,
                Name = "X-ApiKey"
            },
            new AuthenticationTokenSource
            {
                Type = AuthenticationTokenSourceType.Url,
                Name = "apikey"
            },
        };

        public virtual string CreateToken(Dictionary<string, object> claims)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJwtEncoder encoder = new JwtEncoder(algorithm, _serializer, _urlEncoder);

            return encoder.Encode(claims, _secret);
        }

        public virtual Dictionary<string, object> ClaimsFromToken(string token)
        {
            try
            {
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(_serializer, provider);
                IJwtDecoder decoder = new JwtDecoder(_serializer, validator, _urlEncoder);

                return decoder.DecodeToObject<Dictionary<string, object>>(token, _secret, verify: true);
            }
            catch (TokenExpiredException)
            {
                _log.Warn("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                _log.Warn("Token has invalid signature");
            }
            catch (Exception)
            {
                _log.Warn("An invalid Token was passed to the decoder");
            }
            return null;
        }

        public virtual string IdentityFromClaims(Dictionary<string, object> claims)
        {
            if (claims == null) return null;

            return claims.ContainsKey("identity")
                ? claims["identity"].ToString()
                : null;
        }
    }
}
