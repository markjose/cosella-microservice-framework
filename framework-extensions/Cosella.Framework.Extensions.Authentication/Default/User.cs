using Cosella.Framework.Extensions.Authentication.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    public class User : IUser
    {
        private List<string> _roles = new List<string>();

        public User(string identity, string secret, string[] roles)
        {
            Identity = identity;
            Secret = secret;
            _roles = new List<string>(roles);
        }

        public string Identity { get; set; }
        [JsonIgnore]
        public string Secret { get; set; }
        [JsonProperty("secret")]
        public string SecretSetter { set { Secret = value; } }

        public List<string> Roles => _roles;
        public bool IsInRole(string role)
        {
            return _roles.Any(r => r.Equals(role, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}