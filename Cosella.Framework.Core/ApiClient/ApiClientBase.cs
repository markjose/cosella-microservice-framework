using Cosella.Framework.Core.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Cosella.Framework.Core.ApiClient
{
    public abstract class ApiClientBase : IApiClient
    {
        protected string _baseUrl;

        public ApiClientBase(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        private WebClient Client
        {
            get
            {
                var client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                return client;
            }
        }

        public Task<ApiClientResponse<T>> Get<T>(string uri)
        {
            return Task.Run(() =>
            {
                var fullUri = $"{_baseUrl}{uri}";
                try
                {
                    var result = Client.UploadString(new Uri(fullUri), "GET");
                    return ApiResponseOk(JsonConvert.DeserializeObject<T>(result));
                }
                catch (Exception ex)
                {
                    return ApiResponseException<T>(new ApiClientException($"Failed while GET-ting {fullUri}", ex));
                }
            });
        }

        public Task<ApiClientResponse<T>> Delete<T>(string uri)
        {
            return Task.Run(() =>
            {
                var fullUri = $"{_baseUrl}{uri}";
                try
                {
                    var result = Client.UploadString(new Uri(fullUri), "DELETE");
                    return ApiResponseOk(JsonConvert.DeserializeObject<T>(result));
                }
                catch (Exception ex)
                {
                    return ApiResponseException<T>(new ApiClientException($"Failed while DELETE-ing {fullUri}", ex));
                }
            });
        }

        public Task<ApiClientResponse<T>> Post<T>(string uri, object data)
        {
            return Task.Run(() =>
            {
                var fullUri = $"{_baseUrl}{uri}";
                var stringData = data == null ? "" : JsonConvert.SerializeObject(data);
                try
                {
                    var result = Client.UploadString(new Uri(fullUri), "POST", stringData);
                    return ApiResponseOk(JsonConvert.DeserializeObject<T>(result));
                }
                catch (Exception ex)
                {
                    return ApiResponseException<T>(new ApiClientException($"Failed while POST-ting {fullUri}", ex, stringData));
                }
            });
        }

        public Task<ApiClientResponse<T>> Put<T>(string uri, object data)
        {
            return Task.Run(() =>
            {
                var fullUri = $"{_baseUrl}{uri}";
                var stringData = data == null ? "" : JsonConvert.SerializeObject(data);
                try
                {
                    var result = Client.UploadString(new Uri(fullUri), "PUT", stringData);
                    return ApiResponseOk(JsonConvert.DeserializeObject<T>(result));
                }
                catch (Exception ex)
                {
                    return ApiResponseException<T>(new ApiClientException($"Failed while PUT-ting {fullUri}", ex, stringData));
                }
            });
        }

        protected ApiClientResponse<T> ApiResponseException<T>(Exception ex)
        {
            return new ApiClientResponse<T>()
            {
                Status = ApiClientResponseStatus.Exception,
                Exception = ex
            };
        }

        protected ApiClientResponse<T> ApiResponseOk<T>(T payload)
        {
            return new ApiClientResponse<T>()
            {
                Status = ApiClientResponseStatus.Ok,
                Payload = payload
            };
        }
    }
}