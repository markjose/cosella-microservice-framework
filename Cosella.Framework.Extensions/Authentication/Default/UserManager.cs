using Ninject;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    public class UserManager : IUserManager
    {
        private readonly List<User> _users;
        private readonly string[] _allRoles;

        public bool Empty => !_users.Any();

        public UserManager(IKernel kernel)
        {
            _users = new List<User>();
            _allRoles = EnumerateAllRoles();
        }

        private string[] EnumerateAllRoles()
        {
            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .SelectMany(type => type.GetMethods())
                .SelectMany(method => method.GetCustomAttributes<AuthenticationAttribute>())
                .SelectMany(attrib => attrib.Roles)
                .Distinct()
                .ToArray();
        }

        public IEnumerable<User> List()
        {
            return _users;
        }
        public IEnumerable<string> ListRoles()
        {
            return _allRoles;
        }

        public User Authenticate(string username, string password)
        {
            return _users
                .Where(u => u.Username == username && u.Password == password)
                .FirstOrDefault();
        }

        public User Add(User request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new UserException("You must specify a username for the user.");

            if (_users.Any(u => u.Username == request.Username))
                throw new UserException($"A user with the username '{request.Username}' exists.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new UserException($"You must specify a password for the user '{request.Username}'.");

            _users.Add(request);

            return request;
        }

        public User Remove(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user == null) throw new UserException(); // Not found

            _users.Remove(user);
            return user;
        }

        public User SetRoles(string username, string[] roles)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user == null) throw new UserException(); // Not found

            user.Roles = roles ?? new string[0];
            return user;
        }
    }
}
