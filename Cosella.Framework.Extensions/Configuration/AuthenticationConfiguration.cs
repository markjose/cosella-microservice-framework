using System;
using Cosella.Framework.Extensions.Enums;
using Cosella.Framework.Extensions.Models;

namespace Cosella.Framework.Extensions.Configuration
{
    public class AuthenticationConfiguration
    {
        public AuthenticationType AuthenticationType { get; set; }
        public AuthenticationJwtConfiguration Jwt { get; set; } = new AuthenticationJwtConfiguration();
        public Func<string, string, AuthenticatedUser> OnAuthenticate { get; set; }

        public class AuthenticationJwtConfiguration
        {
            public string Secret { get; set; }
        }
    }
}