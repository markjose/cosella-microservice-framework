using Cosella.Framework.Client.Interfaces;
using Cosella.Framework.Core.Logging;
using Cosella.Framework.Core.ServiceDiscovery;
using Cosella.Framework.Core.Workers;
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
        private ILogger _log;
        private IKernel _kernel;
        private IServiceDiscovery _discovery;
        private IServiceRegistration _registration;

        private IInServiceWorker[] _inServiceWorkers;
        private HostControl _hostControl;

        private const int MaxRetries = 10;
        private const int RetryPeriodInSeconds = 1;
        private const int RegistrationCheckPeriodInSeconds = 10;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private HostedService(HostedServiceConfiguration configuration)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            // Create instance name
            configuration.ServiceInstanceName = $"{configuration.ServiceName}{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}";

            _kernel = new StandardKernel();
            _kernel.Bind<HostedServiceConfiguration>().ToMethod(context => configuration).InSingletonScope();

            _kernel.Load(Assembly.GetExecutingAssembly());
            _kernel.Load(new CoreModule());
            _kernel.Load(configuration.Modules.ToArray());

            _log = _kernel.Get<ILogger>();

            _discovery = _kernel.Get<IServiceDiscovery>();
            _log.Debug($"Service instance ID is '{configuration.ServiceInstanceName}'");
        }

        internal static HostedService Create(HostedServiceConfiguration configuration)
        {
            return new HostedService(configuration);
        }

        internal async Task<bool> Start(HostControl hostControl)
        {
            _hostControl = hostControl;

            var startup = _kernel.Get<Startup>();
            var configuration = _kernel.Get<HostedServiceConfiguration>();

            _log.Info("Starting Hosted Microservice...");

            int retries = MaxRetries;
            while (retries > 0)
            {
                var discovery = _kernel.Get<IServiceDiscovery>();
                var deferredRegistration = await _discovery.RegisterServiceDeferred();

                if (configuration.RestApiPort > 0)
                {
                    var apiUri = $"http://{configuration.RestApiHostname}:{configuration.RestApiPort}";
                    try
                    {
                        _app = WebApp.Start(apiUri, startup.Configuration);
                        _log.Info($"Started API at {apiUri}");

                        _registration = await _discovery.RegisterService(deferredRegistration);
                        MonitorServiceRegistration(_cancellationTokenSource.Token);

                        _inServiceWorkers = _kernel.GetAll<IInServiceWorker>().ToArray();
                        if (_inServiceWorkers.Any())
                        {
                            _inServiceWorkers.ToList().ForEach(worker => worker.Start(_cancellationTokenSource.Token));
                            _log.Info($"Startd {_inServiceWorkers.Count()} in service workers");
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        _log.Warn($"Failed to start the API host on {apiUri}: {ex.Message}");
                        do
                        {
                            ex = ex.InnerException;
                            _log.Debug(ex.Message);
                        } while (ex.InnerException != null);

                        _log.Info($"Retrying...({MaxRetries - retries}/{MaxRetries})");
                        configuration.RestApiPort = 0;
                        retries--;
                    }
                }
                else
                {
                    retries = 0;
                }

                await Task.Delay(RetryPeriodInSeconds * 1000, _cancellationTokenSource.Token);
            }

            _discovery.DeregisterService(_registration);
            _log.Fatal($"Could not start API due to one or more fatal errors");

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
            try
            {
                await Task.Delay(RegistrationCheckPeriodInSeconds * 1000, _cancellationToken);

                while (!_cancellationToken.IsCancellationRequested)
                {
                    if (_registration != null)
                    {
                        var serviceInstanceInfo = await _discovery.FindServiceByInstanceName(_registration.InstanceName);
                        if (serviceInstanceInfo == null)
                        {
                            _registration = await _discovery.RegisterService();
                        }
                    }
                    await Task.Delay(RegistrationCheckPeriodInSeconds * 1000, _cancellationToken);
                }
                _log.Info("Service registration monitor stopped.");
            }
            catch (OperationCanceledException)
            {
                _log.Info("Service registration monitor stopped.");
            }
        }
    }
}