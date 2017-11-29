using Cosella.Framework.Client.Interfaces;
using Cosella.Framework.Core.Configuration;
using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Core.Integrations.Consul;
using Cosella.Framework.Core.Integrations.Log4Net;
using Cosella.Framework.Core.Logging;
using Ninject.Modules;

namespace Cosella.Framework.Core
{
    internal class CoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Startup>().To<Startup>().InSingletonScope();
            Bind<IConfigurator>().To<JsonFileConfigurator>().InSingletonScope();
            Bind<IServiceDiscovery>().To<ConsulServiceDiscovery>().InSingletonScope();
            Bind<ILogger>().To<Log4NetLogger>().InSingletonScope();
        }
    }
}