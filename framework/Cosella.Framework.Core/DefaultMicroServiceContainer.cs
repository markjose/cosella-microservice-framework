using Cosella.Framework.Core.Hosting;
using System.Reflection;
using Topshelf;
using Topshelf.ServiceConfigurators;

namespace Cosella.Framework.Core
{
    public class DefaultMicroServiceContainer : IMicroServiceContainer
    {
        private HostedServiceConfiguration _configuration;
        private HostedServiceConfiguration _defaultConfiguration;

        public HostedServiceConfiguration Configuration => _configuration;

        public DefaultMicroServiceContainer()
        {
            string serviceName = Assembly.GetEntryAssembly().GetName().Name.Replace(" ", "");
            _defaultConfiguration = new HostedServiceConfiguration()
            {
                ServiceName = serviceName,
                ServiceDisplayName = serviceName,
                ServiceDescription = "",
                ServiceInstanceName = serviceName
            };
        }

        public virtual void Init(HostedServiceConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual int Run()
        {
            return (int)HostFactory.Run(config =>
            {
                config.UseLog4Net();

                if (_configuration.ServiceName == null) _configuration.ServiceName = _defaultConfiguration.ServiceName;
                if (_configuration.ServiceDisplayName == null) _configuration.ServiceDisplayName = _defaultConfiguration.ServiceDisplayName;
                if (_configuration.ServiceInstanceName == null) _configuration.ServiceInstanceName = _defaultConfiguration.ServiceInstanceName;
                if (_configuration.ServiceDescription == null) _configuration.ServiceDescription = _defaultConfiguration.ServiceDescription;

                config.SetServiceName(_configuration.ServiceName);
                config.SetDisplayName(_configuration.ServiceDisplayName);
                config.SetInstanceName(_configuration.ServiceInstanceName);
                config.SetDescription(_configuration.ServiceDescription);

                config.Service<HostedService>(service =>
                {
                    ServiceConfigurator<HostedService> hostedService = service;
                    hostedService.ConstructUsing(() => HostedService.Create(_configuration));
                    hostedService.WhenStarted((s, hostControl) => s.Start(hostControl).Result);
                    hostedService.WhenStopped(s => s.Stopped());
                    hostedService.WhenPaused(s => s.Paused());
                    hostedService.WhenContinued(s => s.Continued());
                    hostedService.WhenShutdown(s => s.Shutdown());
                });
            });
        }
    }
}
