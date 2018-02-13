using Cosella.Framework.Extensions.Authentication.Interfaces;
using Cosella.Framework.Extensions.Authentication.UserCache;
using Ninject;
using System.Linq;
using System.Threading.Tasks;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    internal class DefaultAuthenticator : IAuthenticator
    {
        private readonly IUserManager _users;
        private readonly IAsyncCache<IUser> _asyncCache;

        public DefaultAuthenticator(IKernel kernel)
        {
            _users = kernel.Get<IUserManager>();
            _asyncCache = kernel.Get<IAsyncCacheFactory>().Get<IUser>();
        }

        public virtual Task<bool> AuthenticateInRole(IUser user, string[] roles, dynamic contextData = null)
        {
            return Task.FromResult(user != null && roles.Any(r => user.IsInRole(r)));
        }

        public virtual Task<IUser> UserFromIdentity(string identity)
        {
            return Task.FromResult(_users.Get(identity));
        }

        public virtual Task<IUser> AuthenticateUser(string identity, string secret)
        {
            return Task.FromResult(_users.Authenticate(identity, secret));
        }
    }
}
