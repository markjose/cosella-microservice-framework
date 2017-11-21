using Cosella.Framework.Core.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cosella.Framework.Core.ApiClient
{
    public abstract class ApiClientBase : IApiClient
    {
        private static HttpClient _staticClient;
        protected HttpClient Client
        {
            get
            {
                if (_staticClient == null)
                {
                    _staticClient = new HttpClient();
                }
                return _staticClient;
            }
        }

        protected string _baseUrl;

        public ApiClientBase(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<ApiClientResponse<T>> Get<T>(string uri)
        {
            var fullUri = $"{_baseUrl}{uri}";
            try
            {
                var response = await Client.GetAsync(fullUri);
                if(response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return ApiResponseOk(JsonConvert.DeserializeObject<T>(content));
                }
                else
                {
                    return ApiResponseError<T>($"Failed while GET-ting {fullUri} ({response.StatusCode})");
                }
            }
            catch (Exception ex)
            {
                return ApiResponseException<T>(new ApiClientException($"Exception while GET-ting {fullUri}, {ex.Message}", ex));
            }
        }

        public async Task<ApiClientResponse<T>> Delete<T>(string uri)
        {
            var fullUri = $"{_baseUrl}{uri}";
            try
            {
                var response = await Client.DeleteAsync(fullUri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return ApiResponseOk(JsonConvert.DeserializeObject<T>(content));
                }
                else
                {
                    return ApiResponseError<T>($"Failed while DELETE-ing {fullUri} ({response.StatusCode})");
                }
            }
            catch (Exception ex)
            {
                return ApiResponseException<T>(new ApiClientException($"Exception while DELETE-ing {fullUri}, {ex.Message}", ex));
            }
        }

        public async Task<ApiClientResponse<T>> Post<T>(string uri, object data)
        {
            var fullUri = $"{_baseUrl}{uri}";
            try
            {
                var response = await Client.PostAsJsonAsync(fullUri, data);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return ApiResponseOk(JsonConvert.DeserializeObject<T>(content));
                }
                else
                {
                    return ApiResponseError<T>($"Failed while POST-ing {fullUri} ({response.StatusCode})");
                }
            }
            catch (Exception ex)
            {
                return ApiResponseException<T>(new ApiClientException($"Exception while POST-ing {fullUri}, {ex.Message}", ex));
            }
        }

        public async Task<ApiClientResponse<T>> Put<T>(string uri, object data)
        {
            var fullUri = $"{_baseUrl}{uri}";
            try
            {
                var response = await Client.PutAsJsonAsync(fullUri, data);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return ApiResponseOk(JsonConvert.DeserializeObject<T>(content));
                }
                else
                {
                    return ApiResponseError<T>($"Failed while PUT-ing {fullUri} ({response.StatusCode})");
                }
            }
            catch (Exception ex)
            {
                return ApiResponseException<T>(new ApiClientException($"Exception while PUT-ing {fullUri}, {ex.Message}", ex));
            }
        }

        protected ApiClientResponse<T> ApiResponseException<T>(Exception ex)
        {
            return new ApiClientResponse<T>()
            {
                Status = ApiClientResponseStatus.Exception,
                Exception = ex
            };
        }

        protected ApiClientResponse<T> ApiResponseError<T>(string error)
        {
            return new ApiClientResponse<T>()
            {
                Status = ApiClientResponseStatus.Error,
                Message = error
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