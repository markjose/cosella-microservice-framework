using System.Linq;
using System.Threading.Tasks;
using Cosella.Framework.Extensions.Authentication.Interfaces;
using Ninject;

namespace Cosella.Services.Gateway
{
    internal class Authenticator : IAuthenticator
    {
        private readonly IUserManager _userManager;

        public Authenticator(IKernel kernel)
        {
            _userManager = kernel.Get<IUserManager>();
        }

        public Task<bool> AuthenticateInRole(IUser user, string[] roles, dynamic contextData = null)
        {
            return Task.FromResult(roles.Any(r => user.Roles.IndexOf(r) >= 0));
        }

        public Task<IUser> AuthenticateUser(string identity, string secret)
        {
            return Task.FromResult(_userManager.Authenticate(identity, secret));
        }

        public Task<IUser> UserFromIdentity(string identity)
        {
            return Task.FromResult(_userManager.Get(identity));
        }
    }
}