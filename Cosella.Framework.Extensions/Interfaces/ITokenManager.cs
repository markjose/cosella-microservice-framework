using Cosella.Framework.Extensions.Models;

namespace Cosella.Framework.Extensions.Interfaces
{
    public interface ITokenManager
    {
        string Create(string username, string password);
        void Verify(string token);
        AuthenticatedUser Decode(string token);
    }
}
