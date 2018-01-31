namespace Cosella.Framework.Extensions.Authentication.Default
{
    public class UserRequest
    {
        public string Identity { get; set; }
        public string Secret { get; set; }
        public string[] Roles { get; set; }
    }
}