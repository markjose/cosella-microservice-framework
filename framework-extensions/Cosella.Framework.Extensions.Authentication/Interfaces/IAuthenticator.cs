using System.Collections.Generic;

namespace Cosella.Framework.Extensions.Authentication.Interfaces
{
    public interface IAuthenticator
    {
        IUser UserFromIdentity(string identity);
        IUser AuthenticateUser(string identity, string secret);
        bool AuthenticateInRole(IUser user, string[] roles, dynamic contextData = null);
    }
}
