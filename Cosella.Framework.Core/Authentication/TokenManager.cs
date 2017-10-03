using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Cosella.Framework.Core.Authentication
{
    internal class TokenManager : ITokenManager
    {
        private readonly AuthenticationConfiguration _config;

        private IJsonSerializer _serializer;
        private IBase64UrlEncoder _urlEncoder;
        private IJwtAlgorithm _algorithm;
        private IDateTimeProvider _provider;
        private IJwtValidator _validator;

        public TokenManager(AuthenticationConfiguration config)
        {
            _config = config;

            _serializer = new JsonNetSerializer();
            _urlEncoder = new JwtBase64UrlEncoder();
            _algorithm = new HMACSHA256Algorithm();
            _provider = new UtcDateTimeProvider();
            _validator = new JwtValidator(_serializer, _provider);
        }

        public string Create(string username, string password)
        {
            AuthenticatedUser user = null;
            if (_config.AuthenticationType == AuthenticationType.Jwt)
            {
                user = _config.OnAuthenticate(username, password);

                if (user != null)
                {
                    var payload = new Dictionary<string, object>
                    {
                        { "username", user.Username },
                        { "name", user.Name },
                        { "roles", user.Roles }
                    };
                    IJwtEncoder encoder = new JwtEncoder(_algorithm, _serializer, _urlEncoder);
                    return encoder.Encode(payload, _config.Jwt.Secret);
                }
            }
            return null;
        }

        public void Verify(string token)
        {
            try
            {
                if (_config.AuthenticationType == AuthenticationType.Jwt)
                {
                    IJwtDecoder decoder = new JwtDecoder(_serializer, _validator, _urlEncoder);
                    var json = decoder.Decode(token, _config.Jwt.Secret, verify: true);
                    ;
                }
                else
                {
                    throw new TokenManagerException(true, "Token was invalid, corrupted or malformed");
                }
            }
            catch (JsonReaderException)
            {
                throw new TokenManagerException(true, "Token was invalid, corrupted or malformed");
            }
            catch (TokenExpiredException)
            {
                throw new TokenManagerException(false, "Token has expired");
            }
            catch (SignatureVerificationException)
            {
                throw new TokenManagerException(false, "Token has invalid signature");
            }
        }

        public AuthenticatedUser Decode(string token)
        {
            try
            {
                if (_config.AuthenticationType == AuthenticationType.Jwt)
                {

                    IJwtDecoder decoder = new JwtDecoder(_serializer, _validator, _urlEncoder);
                    var json = decoder.Decode(token, _config.Jwt.Secret, verify: true);
                    return JsonConvert.DeserializeObject<AuthenticatedUser>(json);
                }
            }
            catch(Exception) { }

            return null;
        }
    }
}
