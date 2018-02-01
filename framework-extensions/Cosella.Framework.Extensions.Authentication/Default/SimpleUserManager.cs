using Cosella.Framework.Extensions.Authentication.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    public class SimpleUserManager : IUserManager
    {
        private readonly List<User> _users;
        private readonly string[] _allRoles;

        public bool Empty => !_users.Any();

        public SimpleUserManager(IKernel kernel)
        {
            _users = new List<User>();
            _allRoles = EnumerateAllRoles();

            var config = kernel.Get<AuthenticationConfiguration>();
            if (config.SimpleUserManagerSeedUsers != null)
            {
                _users.AddRange(config.SimpleUserManagerSeedUsers);
            }
        }

        private string[] EnumerateAllRoles()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a
                    .GetTypes()
                    .SelectMany(type => type.GetMethods())
                    .SelectMany(method => method.GetCustomAttributes<AuthenticationAttribute>())
                    .SelectMany(attrib => attrib.Roles))
                .Distinct()
                .ToArray();
        }

        public IEnumerable<IUser> List()
        {
            return _users;
        }
        public IEnumerable<string> ListRoles()
        {
            return _allRoles;
        }

        public IUser Authenticate(string identity, string secret)
        {
            return _users
                .Where(u => IdentityMatch(u, identity) && u.Secret == secret)
                .FirstOrDefault();
        }

        public IUser Add(string identity, string secret, params string[] roles)
        {
            if (string.IsNullOrWhiteSpace(identity))
                throw new UserException("You must specify a identity for the user.");

            if (_users.Any(u => IdentityMatch(u, identity)))
                throw new UserException($"A user with the identity '{identity}' exists.");

            if (string.IsNullOrWhiteSpace(secret))
                throw new UserException($"You must specify a secret for the user '{identity}'.");

            var newUser = new User(identity, secret, roles);

            _users.Add(newUser);

            return newUser;
        }

        public IUser Remove(string identity)
        {
            var user = _users.FirstOrDefault(u => IdentityMatch(u, identity));
            if (user == null) throw new UserException(); // Not found

            _users.Remove(user);
            return user;
        }

        public IUser SetRoles(string identity, string[] roles)
        {
            var user = _users.FirstOrDefault(u => IdentityMatch(u, identity));
            if (user == null) throw new UserException(); // Not found

            user.Roles.Clear();
            if(roles != null) user.Roles.AddRange(roles);

            return user;
        }

        public IUser Get(string identity)
        {
            return _users.FirstOrDefault(u => IdentityMatch(u, identity));
        }

        private bool IdentityMatch(User user, string identity)
        {
            return user.Identity.Equals(identity, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
