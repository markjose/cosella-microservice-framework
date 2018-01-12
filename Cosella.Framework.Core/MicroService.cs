using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Core.Integrations.Log4Net;
using System;

namespace Cosella.Framework.Core
{
    public class MicroService
    {
        private IMicroServiceContainer _serviceContainer;

        public HostedServiceConfiguration Configuration => _serviceContainer?.Configuration;

        private MicroService(IMicroServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
        }

        private static void ConfigureLog4Net(HostedServiceConfiguration configuration)
        {
            log4net.Config.BasicConfigurator.Configure(
                Log4NetAppenders.GetConsoleAppender(),
                Log4NetAppenders.GetFileAppender(configuration)
            );
        }

        public static MicroService Configure(Action<HostedServiceConfiguration> configurator = null)
        {
            return ConfiguredFor<DefaultMicroServiceContainer>();
        }

        public static MicroService ConfiguredFor<T>(Action<HostedServiceConfiguration> configurator = null)
             where T : IMicroServiceContainer, new()
        {
            var configuration = new HostedServiceConfiguration();
            configurator?.Invoke(configuration);
            ConfigureLog4Net(configuration);

            var serviceContainer = new T();
            serviceContainer.Init(configuration);

            return new MicroService(serviceContainer);
        }

        public int Run()
        {
            return _serviceContainer.Run();
        }
    }
}