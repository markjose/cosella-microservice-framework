namespace Cosella.Contracts
{
    public class AuthenticationResult
    {
        public bool IsAuthorised { get; set; }
        public string Name { get; set; }
        public string[] Roles { get; set; }
        public string Username { get; set; }
    }
}