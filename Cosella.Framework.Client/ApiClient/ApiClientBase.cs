using Cosella.Framework.Client.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cosella.Framework.Client.ApiClient
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

        public Task<ApiClientResponse<T>> Get<T>(string uri)
        {
            return MakeRequest<T>(uri, Client.GetAsync, "GET");
        }

        public Task<ApiClientResponse<T>> Delete<T>(string uri)
        {
            return MakeRequest<T>(uri, Client.DeleteAsync, "DELETE");
        }

        public Task<ApiClientResponse<T>> Post<T>(string uri, object data)
        {
            return MakeDataRequest<T>(uri, Client.PostAsJsonAsync, "POST", data);
        }

        public Task<ApiClientResponse<T>> Put<T>(string uri, object data)
        {
            return MakeDataRequest<T>(uri, Client.PutAsJsonAsync, "PUT", data);
        }

        private async Task<ApiClientResponse<T>> MakeRequest<T>(string uri, Func<string, Task<HttpResponseMessage>> getAsync, string label)
        {
            var fullUri = CombineUrls(_baseUrl, uri);
            try
            {
                var response = await getAsync(fullUri);
                string content = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode
                    ? ApiResponseOk(fullUri, response.StatusCode, JsonConvert.DeserializeObject<T>(content))
                    : ApiResponseError<T>(fullUri, response.StatusCode, response.ReasonPhrase, JsonConvert.DeserializeObject(content));
            }
            catch (Exception ex)
            {
                return ApiResponseException<T>(fullUri, new ApiClientException($"Exception while {label}-ing {fullUri}, {ex.Message}", ex));
            }
        }

        private async Task<ApiClientResponse<T>> MakeDataRequest<T>(string uri, Func<string, object, Task<HttpResponseMessage>> getAsync, string label, object data)
        {
            var fullUri = CombineUrls(_baseUrl, uri);
            try
            {
                var response = await getAsync(fullUri, data);
                string content = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode
                    ? ApiResponseOk(fullUri, response.StatusCode, JsonConvert.DeserializeObject<T>(content))
                    : ApiResponseError<T>(fullUri, response.StatusCode, response.ReasonPhrase, JsonConvert.DeserializeObject(content));
            }
            catch (Exception ex)
            {
                return ApiResponseException<T>(fullUri, new ApiClientException($"Exception while {label}-ing {fullUri}, {ex.Message}", ex));
            }
        }

        private Task<ApiClientResponse<T>> MakeRequest<T>(string uri, Func<string, T, Task<HttpResponseMessage>> postAsJsonAsync, string label, object data)
        {
            throw new NotImplementedException();
        }

        protected ApiClientResponse<T> ApiResponseException<T>(string fullUri, Exception ex)
        {
            return new ApiClientResponse<T>()
            {
                RequestUri = fullUri,
                ResponseStatus = ApiClientResponseStatus.Exception,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = ex.Message,
                Exception = ex
            };
        }

        protected ApiClientResponse<T> ApiResponseError<T>(string fullUri, HttpStatusCode statusCode, string reasonPhrase, object content)
        {
            return new ApiClientResponse<T>()
            {
                RequestUri = fullUri,
                ResponseStatus = ApiClientResponseStatus.Error,
                StatusCode = statusCode,
                Message = reasonPhrase,
                Error = content
            };
        }

        protected ApiClientResponse<T> ApiResponseOk<T>(string fullUri, HttpStatusCode statusCode, T payload)
        {
            return new ApiClientResponse<T>()
            {
                RequestUri = fullUri,
                ResponseStatus = ApiClientResponseStatus.Ok,
                StatusCode = statusCode,
                Payload = payload
            };
        }

        private string CombineUrls(string baseUrl, string uri, bool overlap = false)
        {
            if (overlap == false) return $"{baseUrl}{uri}";

            int rightOffset = 1;
            string endOfUrl = baseUrl.Substring(baseUrl.Length - rightOffset++);

            while (uri.IndexOf(endOfUrl) >= 0)
            {
                endOfUrl = baseUrl.Substring(baseUrl.Length - rightOffset++);
            }
            endOfUrl = endOfUrl.Substring(1);

            var shortenedUri = uri.IndexOf(endOfUrl) == 0 ? uri.Substring(endOfUrl.Length) : uri;
            var combinedUrl = $"{baseUrl}{shortenedUri}";

            return combinedUrl;
        }
    }
}