using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        [JsonConverter(typeof(StringEnumConverter))]
        public ApiClientResponseStatus Status { get; set; }
    }
}