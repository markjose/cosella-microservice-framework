using Cosella.Framework.Client.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net;

namespace Cosella.Framework.Client.ApiClient
{
    public class ServiceRestResponse<T>
    {
        private IServiceInstanceInfo _serviceInstance;
        private ApiClientResponse<T> _apiClientResponse;

        public string ServiceName => _serviceInstance.ServiceName;
        public string ServiceInstance => _serviceInstance.InstanceName;
        public string ServiceNode => _serviceInstance.NodeId;
        public int ServiceVersion => _serviceInstance.Version;

        [JsonConverter(typeof(StringEnumConverter))]
        public ApiClientResponseStatus ResponseStatus => _apiClientResponse.ResponseStatus;
        public string RequestUri => _apiClientResponse.RequestUri;
        public HttpStatusCode StatusCode => _apiClientResponse.StatusCode;
        public string StatusMessage => _apiClientResponse.Message;
        public T Payload => _apiClientResponse.Payload;
        public object Error => _apiClientResponse.Error;
        public Exception Exception => _apiClientResponse.Exception;

        public ServiceRestResponse(ApiClientResponse<T> apiClientResponse, IServiceInstanceInfo serviceInstance)
        {
            _apiClientResponse = apiClientResponse;
            _serviceInstance = serviceInstance;
        }
    }
}