using System.Threading.Tasks;

namespace Cosella.Framework.Extensions.Authentication.Interfaces
{
    public interface IAuthenticator
    {
        Task<IUser> UserFromIdentity(string identity);
        Task<IUser> AuthenticateUser(string identity, string secret);
        Task<bool> AuthenticateInRole(IUser user, string[] roles, dynamic contextData = null);
    }
}
