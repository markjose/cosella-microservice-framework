using System.Collections.Generic;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    public interface IUserManager
    {
        bool Empty { get; }
        IEnumerable<User> List();
        IEnumerable<string> ListRoles();
        User Authenticate(string username, string password);
        User Add(User userRequest);
        User Remove(string username);
        User SetRoles(string username, string[] roles);
    }
}
