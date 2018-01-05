using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Cosella.Framework.Core.Logging;
using Cosella.Framework.Core.Contracts;
using Cosella.Framework.Client.Interfaces;
using Cosella.Framework.Client.ApiClient;

namespace Cosella.Framework.Extensions.Gateway
{
    public class ServiceManager : IServiceManager
    {
        private ILogger _log;
        private IServiceDiscovery _discovery;

        public ServiceManager(ILogger log, IServiceDiscovery discovery)
        {
            _log = log;
            _discovery = discovery;
        }

        public async Task<ServiceDescription[]> GetServiceDescriptions(bool includeServiceDescriptor = false)
        {
            var services = await _discovery.ListServices();

            var groupedServices = services
                .SelectMany(service => service.Instances)
                .Where(instance => instance.Version > 0)
                .GroupBy(service => new
                {
                    service.ServiceName,
                    service.Version
                });

            var descriptors = new Dictionary<string, string>();
            if (includeServiceDescriptor)
            {
                var descriptorTasks = groupedServices
                    .Select(group => group.Any() 
                        ? CreateWebRequest(group.Where(instance => instance.Health == "passing").FirstOrDefault())
                        : null)
                    .Where(task => task != null);

                var results = await Task.WhenAll(descriptorTasks);

                descriptors = results
                    .Where(descriptor => descriptor != null)
                    .ToDictionary(descriptor => $"{descriptor.ServiceName}-{descriptor.Version}", descriptor => descriptor.Schema);
            }

            return groupedServices
                .Select(group =>
                {
                    var firstService = group.First();
                    var serviceVersionKey = $"{group.Key.ServiceName}-{group.Key.Version}";

                    return new ServiceDescription()
                    {
                        ServiceName = firstService.ServiceName,
                        Version = firstService.Version,
                        Instances = group
                            .Select(instance => new ServiceDescription.InstanceDetail()
                            {
                                InstanceName = instance.InstanceName,
                                NodeId = instance.NodeId,
                                Health = instance.Health
                            })
                            .ToArray(),
                        Descriptor = descriptors.ContainsKey(serviceVersionKey)
                            ? JsonConvert.DeserializeObject<dynamic>(descriptors[serviceVersionKey])
                            : null
                    };
                })
                .ToArray();
        }

        public async Task<ServiceRestResponse<object>> ProxyRequest(ServiceProxyRequest request)
        {
            if (string.IsNullOrEmpty(request.ServiceName)) throw new ServiceNotFoundException(request.ServiceName);

            var client = await ServiceRestApiClient.Create(request.ServiceName, _discovery);
            if(client == null) throw new ServiceNotFoundException(request.ServiceName);

            switch (request.Method.ToLowerInvariant())
            {
                case "get":
                    return await client.Get<object>($"{request.Url}{request.QueryString}");
                case "post":
                    return await client.Post<object>($"{request.Url}{request.QueryString}", request.Body);
                case "put":
                    return await client.Put<object>($"{request.Url}{request.QueryString}", request.Body);
                /*case "patch":
                    return await client.Patch<object>($"{request.Url}{request.QueryString}", request.Body);*/
                case "delete":
                    return await client.Delete<object>($"{request.Url}{request.QueryString}");
            }

            return null;
        }

        private Task<ServiceApiDescriptor> CreateWebRequest(IServiceInstanceInfo instanceInfo)
        {
            return Task.Run(() =>
            {
                if (instanceInfo == null) return null;

                var descriptor = new ServiceApiDescriptor()
                {
                    ServiceName = instanceInfo.ServiceName,
                    Version = instanceInfo.Version,
                    Schema = ""
                };

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(instanceInfo.MetadataUri);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    Stream receiveStream = response.GetResponseStream();
                    TextReader reader = new StreamReader(receiveStream, Encoding.UTF8);

                    descriptor.Schema = reader.ReadToEnd();

                    response.Close();
                    reader.Close();
                }
                catch (WebException)
                {
                    return null;
                }

                return descriptor;
            });
        }
    }
}