using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Core.Integrations.Log4Net;
using System;
using System.Reflection;
using Topshelf;
using Topshelf.ServiceConfigurators;

namespace Cosella.Framework.Core
{
    public class MicroService
    {
        private MicroServiceType _containerType;
        private HostedServiceConfiguration _configuration;
        private HostedServiceConfiguration _defaultConfiguration;

        public HostedServiceConfiguration Configuration { get { return _configuration; } }

        private MicroService(
            MicroServiceType containerType,
            HostedServiceConfiguration configuration)
        {
            _containerType = containerType;
            _configuration = configuration;

            string serviceName = Assembly.GetEntryAssembly().GetName().Name.Replace(" ", "");
            _defaultConfiguration = new HostedServiceConfiguration()
            {
                ServiceName = serviceName,
                ServiceDisplayName = serviceName,
                ServiceDescription = "",
                ServiceInstanceName = serviceName
            };
        }

        private static void ConfigureLog4Net(HostedServiceConfiguration configuration)
        {
            log4net.Config.BasicConfigurator.Configure(
                Log4NetAppenders.GetConsoleAppender(),
                Log4NetAppenders.GetFileAppender(configuration)
            );
        }

        public static MicroService ConfiguredFor(
            MicroServiceType containerType,
            Action<HostedServiceConfiguration> configurator = null)
        {
            var configuration = new HostedServiceConfiguration();
            configurator?.Invoke(configuration);
            ConfigureLog4Net(configuration);
            return new MicroService(containerType, configuration);
        }

        public int Run()
        {
            if (_containerType == MicroServiceType.WindowsConsole
                || _containerType == MicroServiceType.WindowsService)
            {
                return (int)HostFactory.Run(config =>
                {
                    config.UseLog4Net();

                    config.SetServiceName(_configuration.ServiceName ?? _defaultConfiguration.ServiceName);
                    config.SetDisplayName(_configuration.ServiceDisplayName ?? _defaultConfiguration.ServiceDisplayName);
                    config.SetInstanceName(_configuration.ServiceInstanceName ?? _defaultConfiguration.ServiceInstanceName);
                    config.SetDescription(_configuration.ServiceDescription ?? _defaultConfiguration.ServiceDescription);

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

            throw new NotImplementedException($"The container type '{_containerType}' is not currently implemented.");
        }
    }
}