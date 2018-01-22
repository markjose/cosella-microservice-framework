namespace Cosella.Framework.Extensions.Gateway
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object Error { get; set; }
    }
}