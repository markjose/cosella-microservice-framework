using Cosella.Framework.Extensions.Authentication.Interfaces;
using Ninject;
using System.Linq;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    internal class DefaultAuthenticator : IAuthenticator
    {
        private readonly IUserManager _users;

        public DefaultAuthenticator(IKernel kernel)
        {
            _users = kernel.Get<IUserManager>();
        }

        public virtual bool AuthenticateInRole(IUser user, string[] roles, dynamic contextData = null)
        {
            return user != null && roles.Any(r => user.IsInRole(r));
        }

        public virtual IUser UserFromIdentity(string identity)
        {
            return _users.Get(identity);
        }

        public virtual IUser AuthenticateUser(string identity, string secret)
        {
            return _users.Authenticate(identity, secret);
        }
    }
}
