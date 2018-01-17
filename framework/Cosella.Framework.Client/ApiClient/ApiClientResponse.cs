using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net;

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
        public string Message { get; set; }
        public T Payload { get; set; }

        public string RequestUri { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ApiClientResponseStatus ResponseStatus { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object Error { get; set; }
        public Exception Exception { get; set; }
    }
}