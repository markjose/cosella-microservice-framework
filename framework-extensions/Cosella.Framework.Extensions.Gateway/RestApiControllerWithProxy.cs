﻿using Cosella.Framework.Client.ApiClient;
using Cosella.Framework.Client.Interfaces;
using Cosella.Framework.Core.Controllers;
using Ninject;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cosella.Framework.Extensions.Gateway
{
    public class RestApiControllerWithProxy : RestApiController
    {
        private const string BaseUrl = "/api/v";
        private IServiceDiscovery _discovery;
        private Dictionary<string, ServiceRestApiClient> _clients = new Dictionary<string, ServiceRestApiClient>();

        public RestApiControllerWithProxy(IKernel kernel)
        {
            _discovery = kernel.Get<IServiceDiscovery>();
        }

        protected async Task<IHttpActionResult> ProxyStreamGet(string serviceName, int serviceVersion, string apiPath)
        {
            var client = await GetClient(serviceName);
            if (client == null)
            {
                return Content(HttpStatusCode.ServiceUnavailable, $"The service '{serviceName}' is currently unavailable.");
            }

            var url = $"{BaseUrl}{serviceVersion}/{apiPath}";
            try
            {
                var response = await client.GetStream(url);
                var contentStream = await response.Content.ReadAsStreamAsync();
                return new ProxyStreamResult(contentStream, response.Content.Headers.ContentType.MediaType);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected async Task<IHttpActionResult> ProxyStreamPost(string serviceName, int serviceVersion, string apiPath, object data)
        {
            var client = await GetClient(serviceName);
            if (client == null)
            {
                return Content(HttpStatusCode.ServiceUnavailable, $"The service '{serviceName}' is currently unavailable.");
            }

            var url = $"{BaseUrl}{serviceVersion}/{apiPath}";
            try
            {
                var response = await client.PostStream(url, data);
                var contentStream = await response.Content.ReadAsStreamAsync();
                return new ProxyStreamResult(contentStream, response.Content.Headers.ContentType.MediaType);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected async Task<IHttpActionResult> ProxyRestGet<TResult>(string serviceName, int serviceVersion, string apiPath)
        {
            var client = await GetClient(serviceName);
            if (client == null)
            {
                return Content(HttpStatusCode.ServiceUnavailable, $"The service '{serviceName}' is currently unavailable.");
            }

            var url = $"{BaseUrl}{serviceVersion}/{apiPath}";
            var response = await client.Get<TResult>(url);

            return HandleResponse(response);
        }

        protected async Task<IHttpActionResult> ProxyRestDelete<TResult>(string serviceName, int serviceVersion, string apiPath)
        {
            var client = await GetClient(serviceName);
            if (client == null)
            {
                return Content(HttpStatusCode.ServiceUnavailable, $"The service '{serviceName}' is currently unavailable.");
            }

            var url = $"{BaseUrl}{serviceVersion}/{apiPath}";
            var response = await client.Delete<TResult>(url);

            return HandleResponse(response);
        }

        protected async Task<IHttpActionResult> ProxyRestPost<TRequestData, TResult>(string serviceName, int serviceVersion, string apiPath, TRequestData data) where TRequestData : class
        {
            var client = await GetClient(serviceName);
            if (client == null)
            {
                return Content(HttpStatusCode.ServiceUnavailable, $"The service '{serviceName}' is currently unavailable.");
            }

            var url = $"{BaseUrl}{serviceVersion}/{apiPath}";
            var response = await client.Post<TResult>(url, data);

            return HandleResponse(response);
        }

        protected async Task<IHttpActionResult> ProxyRestPut<TRequestData, TResult>(string serviceName, int serviceVersion, string apiPath, TRequestData data) where TRequestData : class
        {
            var client = await GetClient(serviceName);
            if (client == null)
            {
                return Content(HttpStatusCode.ServiceUnavailable, $"The service '{serviceName}' is currently unavailable.");
            }

            var url = $"{BaseUrl}{serviceVersion}/{apiPath}";
            var response = await client.Put<TResult>(url, data);

            return HandleResponse(response);
        }

        protected async Task<IHttpActionResult> ProxyRestPatch<TRequestData, TResult>(string serviceName, int serviceVersion, string apiPath, TRequestData data) where TRequestData : class
        {
            var client = await GetClient(serviceName);
            if (client == null)
            {
                return Content(HttpStatusCode.ServiceUnavailable, $"The service '{serviceName}' is currently unavailable.");
            }

            var url = $"{BaseUrl}{serviceVersion}/{apiPath}";
            var response = await client.Patch<TResult>(url, data);

            return HandleResponse(response);
        }

        private async Task<ServiceRestApiClient> GetClient(string serviceName)
        {
            if (_clients.ContainsKey(serviceName)) return _clients[serviceName];

            var client = await ServiceRestApiClient.Create(serviceName, _discovery);
            if (client != null) _clients.Add(serviceName, client);
            return client;
        }

        private IHttpActionResult HandleResponse<TResult>(ServiceRestResponse<TResult> response)
        {
            if(response.ResponseStatus != ApiClientResponseStatus.Ok)
            {
                return Content(response.StatusCode, new ErrorResponse() {
                    StatusCode = (int)response.StatusCode,
                    StatusMessage = response.StatusMessage,
                    Error = response.ResponseStatus == ApiClientResponseStatus.Error
                        ? response.Error
                        : response.Exception
                });
            }
            return Content(response.StatusCode, response.Payload);
        }
    }
}
