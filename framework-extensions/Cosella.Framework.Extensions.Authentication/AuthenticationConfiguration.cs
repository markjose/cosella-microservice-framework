using Cosella.Framework.Extensions.Authentication.Default;
using System.Collections.Generic;

namespace Cosella.Framework.Extensions.Authentication
{
    public class AuthenticationConfiguration
    {
        public bool EnableSimpleTokenController { get; set; } = false;
        public string SimpleTokenControllerSecret { get; set; } = null;

        public bool EnableSimpleUserManager { get; set; } = false;
        public List<User> SimpleUserManagerSeedUsers { get; set; } = new List<User>();
    }
}
