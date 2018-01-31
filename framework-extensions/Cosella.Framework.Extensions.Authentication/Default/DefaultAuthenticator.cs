using Cosella.Framework.Extensions.Authentication.Interfaces;
using System.Linq;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    internal class DefaultAuthenticator : IAuthenticator
    {
        private readonly IUserManager _users;

        public DefaultAuthenticator(IUserManager users)
        {
            _users = users;
        }

        public bool AuthenticateInRole(IUser user, string[] roles, dynamic contextData = null)
        {
            return user != null && roles.Any(r => user.IsInRole(r));
        }

        public IUser UserFromIdentity(string identity)
        {
            return _users.Get(identity);
        }

        public IUser AuthenticateUser(string identity, string secret)
        {
            return _users.Authenticate(identity, secret);
        }
    }
}
