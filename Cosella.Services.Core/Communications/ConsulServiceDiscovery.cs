using System;
using Cosella.Services.Core.Hosting;
using Cosella.Services.Core.Interfaces;
using Cosella.Services.Core.Models;
using System.Collections.Generic;
using log4net;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Cosella.Services.Core.Communications
{
    public class ConsulServiceDiscovery : IServiceDiscovery
    {
        public const int DefaultPortNumber = 5000;

        private readonly ILog _log;
        private readonly HostedServiceConfiguration _configuration;
        private readonly IApiClient _client;

        public ConsulServiceDiscovery(ILog log, HostedServiceConfiguration configuration)
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
                    DeregisterCriticalServiceAfter = "15m",
                    Http = $"http://{_configuration.RestApiHostname}:{_configuration.RestApiPort}/status?instanceId={_configuration.ServiceInstanceName}",
                    Interval = "10s"
                }
            });
        }

        public IServiceRegistration RegisterService()
        {
            return RegisterService(RegisterServiceDeferred());
        }

        public IServiceRegistration RegisterService(Task<ApiClientResponse<string>> registrationTask)
        {
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
            var response = await _client.Get<ConsulServices>("/agent/services");
            var services = new List<IServiceInfo>();
            foreach (ConsulServiceInfo info in response.Payload.Values)
            {
                /*
                 * "{\r\n  \"Exception\": null,\r\n  \"Payload\": {\r\n    \"Authenticator20170606154749674\":
                 * {\r\n      \"ID\": \"Authenticator20170606154749674\",\r\n
                 * \"Service\": \"Authenticator\",\r\n
                 * \"Tags\": [\r\n        \"v1\"\r\n      ],\r\n
                 * \"Address\": \"91.224.190.216\",\r\n
                 *  \"Port\": 5001,\r\n
                 *   \"EnableTagOverride\": false,\r\n
                 *    \"CreateIndex\": 0,\r\n
                 *     \"ModifyIndex\": 0\r\n    },\r\n
                 *      \"Gateway20170606154749644\": {\r\n      \"ID\": \"Gateway20170606154749644\",\r\n      \"Service\": \"Gateway\",\r\n      \"Tags\": [\r\n        \"v1\"\r\n      ],\r\n      \"Address\": \"91.224.190.216\",\r\n      \"Port\": 5000,\r\n      \"EnableTagOverride\": false,\r\n      \"CreateIndex\": 0,\r\n      \"ModifyIndex\": 0\r\n    },\r\n    \"consul\": {\r\n      \"ID\": \"consul\",\r\n      \"Service\": \"consul\",\r\n      \"Tags\": [],\r\n      \"Address\": \"\",\r\n      \"Port\": 8300,\r\n      \"EnableTagOverride\": false,\r\n      \"CreateIndex\": 0,\r\n      \"ModifyIndex\": 0\r\n    }\r\n
  },\r\n  \"Status\": 0\r\n}"
                 */
                Console.Write(info);
            }
            return services.ToArray();
        }

        public IServiceRegistration FindServiceByName(string serviceName)
        {
            throw new NotImplementedException();
        }

        public IServiceRegistration FindServiceByInstanceId(string serviceName)
        {
            throw new NotImplementedException();
        }
    }
}