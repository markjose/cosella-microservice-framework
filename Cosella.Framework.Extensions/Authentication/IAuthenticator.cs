namespace Cosella.Framework.Extensions.Authentication
{
    public interface IAuthenticator
    {
        AuthenticatedUser UserFromToken(string token);
        AuthenticatedUser UserFromCredentials(string userId, string secret);
        string TokenFromUser(AuthenticatedUser user);
    }
}
