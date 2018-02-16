using Cosella.Framework.Extensions.Authentication.Interfaces;
using Cosella.Framework.Extensions.Authentication.UserCache;
using Ninject;
using System;
using System.Collections.Generic;
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
            _asyncCache = kernel.Get<IAsyncCacheFactory>().Create(fetchUsersFunc, getUserKey);
        }

        private string getUserKey(IUser user)
        {
            return user.Identity;
        }

        private IEnumerable<IUser> fetchUsersFunc(IEnumerable<string> identities)
        {
            return identities.Select(i => _users.Get(i));
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
