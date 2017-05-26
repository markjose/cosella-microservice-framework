using log4net;
using Microsoft.Owin.Hosting;
using Ninject;
using System;
using System.Reflection;

namespace Cosella.Services.Core.Hosting
{
    internal class HostedService
    {
        private IDisposable _app;
        private IKernel _kernel;

        private HostedService(HostedServiceConfiguration configuration)
        {
            ILog log = LogManager.GetLogger(Assembly.GetEntryAssembly().GetName().Name);

            _kernel = new StandardKernel();
            _kernel.Load(Assembly.GetExecutingAssembly());
            _kernel.Bind<Startup>().To<Startup>().InSingletonScope();
            _kernel.Bind<HostedServiceConfiguration>().ToMethod(context => configuration).InSingletonScope();
            _kernel.Bind<ILog>().ToMethod(context => log).InSingletonScope();
        }

        internal static HostedService Create(HostedServiceConfiguration configuration)
        {
            return new HostedService(configuration);
        }

        internal bool Start()
        {
            var startup = _kernel.Get<Startup>();
            var configuration = _kernel.Get<HostedServiceConfiguration>();

            _app = WebApp.Start($"http://localhost:{configuration.RestApiPort}/", startup.Configuration);
            return true;
        }

        internal bool Stopped()
        {
            _app.Dispose();
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
    }
}