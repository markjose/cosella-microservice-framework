﻿using Cosella.Framework.Core.Configuration;
using Cosella.Framework.Core.Integrations.Consul;
using Cosella.Framework.Core.ServiceDiscovery;
using Cosella.Framework.Core.Workers;
using log4net;
using Microsoft.Owin.Hosting;
using Ninject;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace Cosella.Framework.Core.Hosting
{
    internal class HostedService
    {
        private IDisposable _app;
        private ILog _log;
        private IKernel _kernel;
        private IServiceDiscovery _discovery;
        private IServiceRegistration _registration;

        private IInServiceWorker[] _inServiceWorkers;

        private const int MaxRetries = 10;
        private const int RetryPeriodInSeconds = 1;
        private const int RegistrationCheckPeriodInSeconds = 10;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private HostedService(HostedServiceConfiguration configuration)
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            _cancellationTokenSource = new CancellationTokenSource();

            // Create instance name
            configuration.ServiceInstanceName = $"{configuration.ServiceName}{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}";

            // Create logger for this instance
            _log = LogManager.GetLogger(entryAssembly, configuration.ServiceInstanceName);
            _log.Debug($"Service instance ID is '{configuration.ServiceInstanceName}'");

            _kernel = new StandardKernel();
            _kernel.Bind<HostedServiceConfiguration>().ToMethod(context => configuration).InSingletonScope();
            _kernel.Bind<ILog>().ToMethod(context => _log).InSingletonScope();

            _kernel.Load(Assembly.GetExecutingAssembly());
            _kernel.Load(configuration.Modules.ToArray());

            _discovery = _kernel.Get<IServiceDiscovery>();
        }

        internal static HostedService Create(HostedServiceConfiguration configuration)
        {
            return new HostedService(configuration);
        }

        internal bool Start(HostControl hostControl)
        {
            var startup = _kernel.Get<Startup>();
            var configuration = _kernel.Get<HostedServiceConfiguration>();

            _log.Info("Starting Hosted Microservice...");

            int retries = MaxRetries;
            while (retries > 0)
            {
                var discovery = _kernel.Get<IServiceDiscovery>();
                var registrationTask = _discovery.RegisterServiceDeferred();

                if (configuration.RestApiPort > 0)
                {
                    var apiUri = $"http://*:{configuration.RestApiPort}";
                    try
                    {
                        _app = WebApp.Start(apiUri, startup.Configuration);
                        _log.Info($"Started API at {apiUri}");

                        _registration = _discovery.RegisterService(registrationTask);
                        MonitorServiceRegistration(_cancellationTokenSource.Token);

                        _inServiceWorkers = _kernel.GetAll<IInServiceWorker>().ToArray();
                        if (_inServiceWorkers.Any()) {
                            _inServiceWorkers.ToList().ForEach(worker => worker.Start(_cancellationTokenSource.Token));
                            _log.Info($"Startd {_inServiceWorkers.Count()} in service workers");
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        _log.Debug($"Failed to start the API {ex.Message}");
                        _log.Warn($"Failed to start the API host on port {configuration.RestApiPort}");
                        _log.Info($"Retrying...({MaxRetries - retries}/{MaxRetries})");
                        configuration.RestApiPort = 0;
                        retries--;
                    }
                }
                else
                {
                    retries = 0;
                }

                Task.Delay(RetryPeriodInSeconds * 1000).Wait();
            }

            _discovery.DeregisterService(_registration);
            _log.Fatal($"Could not start API as the ports specified were invalid");
            return false;
        }

        internal bool Stopped()
        {
            _cancellationTokenSource.Cancel();
            if (_inServiceWorkers != null)
            {
                _inServiceWorkers.ToList().ForEach(worker => worker.Stop());
            }
            _discovery.DeregisterService(_registration);
            if (_app != null)
            {
                _app.Dispose();
            }
            return true;
        }

        internal bool Paused()
        {
            return false;
        }

        internal bool Continued()
        {
            return false;
        }

        internal bool Shutdown()
        {
            return false;
        }

        private async void MonitorServiceRegistration(CancellationToken _cancellationToken)
        {
            Task.Delay(RegistrationCheckPeriodInSeconds * 1000).Wait();

            while (!_cancellationToken.IsCancellationRequested)
            {
                if (_registration != null)
                {
                    var serviceInstanceInfo = await _discovery.FindServiceByInstanceName(_registration.InstanceName);
                    if (serviceInstanceInfo == null)
                    {
                        _registration = _discovery.RegisterService();
                    }
                }
                Task.Delay(RegistrationCheckPeriodInSeconds * 1000).Wait();
            }
        }
    }
}