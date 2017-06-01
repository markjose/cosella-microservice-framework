using System;

namespace Cosella.Services.Core.Communications
{
    internal class ConsulException : Exception
    {
        private string _postData;

        public string PostData { get { return _postData; } }

        public ConsulException(string message, Exception ex, string postData = "")
            : base(message, ex)
        {
            _postData = postData;
        }
    }
}