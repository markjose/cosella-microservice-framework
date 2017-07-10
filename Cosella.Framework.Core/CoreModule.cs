namespace Cosella.Framework.Core
{
    using Configuration;
    using Hosting;
    using Integrations.Consul;
    using log4net;
    using Ninject.Modules;
    using ServiceDiscovery;
    using VersionTracking;

    public class CoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Startup>().To<Startup>().InSingletonScope();
            Bind<IConfigurator>().To<JsonFileConfigurator>().InSingletonScope();
            Bind<IServiceDiscovery>().To<ConsulServiceDiscovery>().InSingletonScope();
        }
    }
}