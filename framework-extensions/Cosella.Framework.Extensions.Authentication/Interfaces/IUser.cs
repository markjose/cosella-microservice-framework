using System.Collections.Generic;

namespace Cosella.Framework.Extensions.Authentication.Interfaces
{
    public interface IUser
    {
        string Identity { get; }
        List<string> Roles { get; }
        bool IsInRole(string role);
    }
}
