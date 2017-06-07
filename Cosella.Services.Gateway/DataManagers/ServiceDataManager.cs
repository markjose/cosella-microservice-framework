namespace Cosella.Services.Gateway.DataManagers
{
    using Contracts;
    using Core.ServiceDiscovery;
    using Interfaces;
    using log4net;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Net;
    using System.IO;
    using System.Text;
    using Models;
    using Newtonsoft.Json;

    public class ServiceDataManager : IServiceDataManager
    {
        private ILog _log;
        private IServiceDiscovery _discovery;

        public ServiceDataManager(ILog log, IServiceDiscovery discovery)
        {
            _log = log;
            _discovery = discovery;
        }

        public async Task<ServiceDescription[]> GetServiceDescriptions(bool includeServiceDescriptor = false)
        {
            var services = await _discovery.ListServices();

            var groupedServices = services
                .SelectMany(service => service.Instances)
                .Where(service => service.Version > 0)
                .GroupBy(service => new
                {
                    ServiceName = service.ServiceName,
                    Version = service.Version
                });

            var descriptors = new Dictionary<string, string>();
            if (includeServiceDescriptor)
            {
                var descriptorTasks = groupedServices.Select(group => CreateWebRequest(group.First()));
                await Task.WhenAll(descriptorTasks);

                descriptors = descriptorTasks
                    .Select(task => task.Result)
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

        private Task<ServiceApiDescriptor> CreateWebRequest(IServiceInstanceInfo instanceInfo)
        {
            return Task.Run(() =>
            {
                var descriptor = new ServiceApiDescriptor()
                {
                    ServiceName = instanceInfo.ServiceName,
                    Version = instanceInfo.Version,
                    Schema = ""
                };

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(instanceInfo.MetadataUri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream receiveStream = response.GetResponseStream();
                TextReader reader = new StreamReader(receiveStream, Encoding.UTF8);

                descriptor.Schema = reader.ReadToEnd();

                response.Close();
                reader.Close();

                return descriptor;
            });
        }
    }
}