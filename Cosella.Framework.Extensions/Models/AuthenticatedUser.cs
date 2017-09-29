namespace Cosella.Framework.Extensions.Models
{
    public class AuthenticatedUser
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string[] Roles { get; set; }
    }
}