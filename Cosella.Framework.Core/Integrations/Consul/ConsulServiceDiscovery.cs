using Cosella.Framework.Core.ApiClient;
using Cosella.Framework.Core.Configuration;
using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Core.Logging;
using Cosella.Framework.Core.ServiceDiscovery;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cosella.Framework.Core.Integrations.Consul
{
    public class ConsulServiceDiscovery : IServiceDiscovery
    {
        public const int DefaultPortNumber = 5000;

        private readonly ILogger _log;
        private readonly HostedServiceConfiguration _configuration;
        private readonly IApiClient _client;

        public ConsulServiceDiscovery(ILogger log, HostedServiceConfiguration configuration)
        {
            _log = log;
            _configuration = configuration;
            _client = new ConsulApiClient();
        }

        public void DeregisterService(IServiceRegistration registration)
        {
            if (registration != null)
            {
                _log.Warn($"Deregistering service '{registration.InstanceName}' from discovery...");

                var deregistrationTask = _client.Put<string>($"/agent/service/deregister/{registration.InstanceName}", null);

                //Do deregistration
                try
                {
                    deregistrationTask.Wait();
                    _log.Info($"De-registration complete.");
                }
                catch (Exception ex)
                {
                    _log.Warn($"De-registration failed: {ex.Message}");
                }
            }
        }

        public Task<ApiClientResponse<string>> RegisterServiceDeferred()
        {
            // We don't want to do any of this stuff is we've disabled service discovery or registration
            if(_configuration.DisableRegistration || _configuration.DisableServiceDiscovery) 
            {
                return Task.FromResult(default(ApiClientResponse<string>));
            }

            // Auto configure Hostname
            if (string.IsNullOrWhiteSpace(_configuration.RestApiHostname))
            {
                _configuration.RestApiHostname = DetermineBestHostAddress();
            }
            _log.Debug($"Service is available at '{_configuration.RestApiHostname}'");

            // Auto configure Port number
            if (_configuration.RestApiPort <= 0)
            {
                _log.Debug($"Querying {_configuration.ServiceName} services for available ports");

                var response = _client
                    .Get<ConsulServices>("/agent/services")
                    .Result;

                if (response.Status == ApiClientResponseStatus.Exception)
                {
                    _log.Error($"Failed to query auto port for service instance '{_configuration.ServiceInstanceName}'");
                    _log.Error(response.Exception.Message);
                    return null;
                }

                var services = response
                    .Payload
                    .Values
                    .Where(service => service.Address.Equals(_configuration.RestApiHostname, StringComparison.InvariantCultureIgnoreCase));

                _configuration.RestApiPort = services.Any()
                    ? services.GroupBy(service => service.Address).Max(group => group.Max(service => service.Port)) + 1
                    : DefaultPortNumber;
            }
            _log.Debug($"Service is available on port '{_configuration.RestApiPort}'");

            // Task to register with Consul agent
            _log.Info($"Registering service '{_configuration.ServiceName}' for discovery...");
            return _client.Put<string>("/agent/service/register", new ConsulRegistrationRequest()
            {
                Id = _configuration.ServiceInstanceName,
                Name = _configuration.ServiceName,
                Address = _configuration.RestApiHostname,
                Port = _configuration.RestApiPort,
                EnableTagOverride = false,
                Tags = new List<string>()
                {
                    $"v{_configuration.RestApiVersion}"
                },
                Check = new ConsulHealthCheck()
                {
                    DeregisterCriticalServiceAfter = "5m",
                    Http = $"http://{_configuration.RestApiHostname}:{_configuration.RestApiPort}/status?instanceId={_configuration.ServiceInstanceName}",
                    Interval = "10s"
                }
            });
        }

        public IServiceRegistration RegisterService()
        {
            // We don't want to do any of this stuff is we've disabled service discovery or registration
            if(_configuration.DisableRegistration || _configuration.DisableServiceDiscovery) 
            {
                return null;
            }

            return RegisterService(RegisterServiceDeferred());
        }

        public IServiceRegistration RegisterService(Task<ApiClientResponse<string>> registrationTask)
        {
            // We don't want to do any of this stuff is we've disabled service discovery or registration
            if(_configuration.DisableRegistration || _configuration.DisableServiceDiscovery) 
            {
                _log.Warn($"Registration disabled, not registered.");
                return null;
            }

            //Do registration
            try
            {
                registrationTask.Wait();
                _log.Info($"Registration complete.");
                return new ServiceRegistration()
                {
                    InstanceName = _configuration.ServiceInstanceName
                };
            }
            catch (Exception ex)
            {
                _log.Warn($"Registration failed: {ex.Message}");
            }

            return null;
        }

        private string DetermineBestHostAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return "localhost";
            }

            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }

            return localIP;
        }

        public async Task<IServiceInfo[]> ListServices()
        {
            // We don't want to do any of this stuff is we've disabled service discovery
            if(_configuration.DisableServiceDiscovery) 
            {
                _log.Warn($"Service discovery is disabled.");
                return new ServiceInfo[0];
            }

            var servicesTask = _client.Get<ConsulServices>("/agent/services");
            var checksTask = _client.Get<ConsulServiceChecks>("/agent/checks");

            await Task.WhenAll(servicesTask, checksTask);

            var services = new Dictionary<string, IServiceInfo>();

            if (servicesTask.Result.Status == ApiClientResponseStatus.Exception)
            {
                throw servicesTask.Result.Exception;
            }

            if (checksTask.Result.Status == ApiClientResponseStatus.Exception)
            {
                throw checksTask.Result.Exception;
            }

            foreach (ConsulServiceInfo info in servicesTask.Result.Payload.Values)
            {
                var healthCheckKey = $"service:{info.Id}";

                ConsulServiceHealth healthCheck = null;
                if (checksTask.Result.Payload.ContainsKey(healthCheckKey))
                {
                    healthCheck = checksTask.Result.Payload[healthCheckKey];
                }

                if (!services.ContainsKey(info.Service))
                {
                    services.Add(info.Service, new ServiceInfo() { ServiceName = info.Service });
                }
                int version = ParseVersionFromTags(info.Tags);
                services[info.Service].Instances.Add(new ServiceInstanceInfo()
                {
                    ServiceName = info.Service,
                    InstanceName = info.Id,
                    NodeId = healthCheck?.Node,
                    Health = healthCheck?.Status,
                    MetadataUri = $"http://{info.Address}:{info.Port}/swagger/docs/v{version}",
                    StatusUri = $"http://{info.Address}:{info.Port}/status?instanceId={info.Id}",
                    ApiUri = $"http://{info.Address}:{info.Port}/api/v{version}/",
                    Version = version
                });
            }
            return services.Values.ToArray();
        }

        private int ParseVersionFromTags(List<string> tags)
        {
            if (tags == null || tags.Count < 1)
            {
                return 0;
            }

            return int.Parse(
                Regex.Match(tags.First(), @"\d+").Value,
                NumberFormatInfo.InvariantInfo);
        }

        public async Task<IServiceInstanceInfo> FindServiceByName(string serviceName)
        {
            // We don't want to do any of this stuff is we've disabled service discovery
            if(_configuration.DisableServiceDiscovery) 
            {
                _log.Warn($"Service discovery is disabled.");
                return default(ServiceInstanceInfo);
            }

            try
            {
                var services = await ListServices();

                return services
                    .Where(service => service.ServiceName.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase))
                    .SelectMany(service => service.Instances)
                    .Where(instance => instance.Health.Equals("passing", StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
            }
            catch (ApiClientException ex)
            {
                _log.Error(ex.Message);
                return null;
            }
        }

        public async Task<IServiceInstanceInfo> FindServiceByInstanceName(string instanceName)
        {
            // We don't want to do any of this stuff is we've disabled service discovery
            if(_configuration.DisableServiceDiscovery) 
            {
                _log.Warn($"Service discovery is disabled.");
                return default(ServiceInstanceInfo);
            }

            try
            {
                var services = await ListServices();

                return services
                    .SelectMany(service => service.Instances)
                    .Where(instance => instance.InstanceName.Equals(instanceName, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
            }
            catch (ApiClientException ex)
            {
                _log.Error(ex.Message);
                return null;
            }
        }
    }
}