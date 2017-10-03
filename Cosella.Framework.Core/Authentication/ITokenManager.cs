namespace Cosella.Framework.Core.Authentication
{
    public interface ITokenManager
    {
        string Create(string username, string password);
        void Verify(string token);
        AuthenticatedUser Decode(string token);
    }
}
