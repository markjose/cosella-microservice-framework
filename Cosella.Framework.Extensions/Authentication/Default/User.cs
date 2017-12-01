using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    public class User
    {

        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonProperty("password")]
        public string PasswordSetter { set { Password = value; } }
        public IEnumerable<string> Roles { get; set; } = new string[0];
    }
}