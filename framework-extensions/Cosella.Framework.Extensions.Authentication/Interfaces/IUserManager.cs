using System.Collections.Generic;

namespace Cosella.Framework.Extensions.Authentication.Interfaces
{
    public interface IUserManager
    {
        bool Empty { get; }
        IEnumerable<IUser> List();
        IEnumerable<string> ListRoles();
        IUser Authenticate(string identity, string secret);
        IUser Add(string identity, string secret, params string[] roles);
        IUser Get(string identity);
        IUser Remove(string identity);
        IUser SetRoles(string identity, string[] roles);
    }
}
