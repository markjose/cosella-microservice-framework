using System;

namespace Cosella.Framework.Core.ApiClient
{
    internal class ApiClientException : Exception
    {
        private string _postData;

        public string PostData { get { return _postData; } }

        public ApiClientException(string message, Exception ex, string postData = "")
            : base(message, ex)
        {
            _postData = postData;
        }
    }
}