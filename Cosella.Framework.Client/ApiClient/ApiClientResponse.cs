using System;

namespace Cosella.Framework.Client.ApiClient
{
    public enum ApiClientResponseStatus
    {
        Ok,
        Exception,
        Error
    }

    public class ApiClientResponse<T>
    {
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public T Payload { get; set; }
        public ApiClientResponseStatus Status { get; set; }
    }
}