namespace Cosella.Framework.Core
{
    using Configuration;
    using Cosella.Framework.Core.Authentication;
    using Cosella.Framework.Core.Integrations.Log4Net;
    using Cosella.Framework.Core.Logging;
    using Hosting;
    using Integrations.Consul;
    using Ninject.Modules;
    using ServiceDiscovery;

    internal class CoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Startup>().To<Startup>().InSingletonScope();
            Bind<IConfigurator>().To<JsonFileConfigurator>().InSingletonScope();
            Bind<IServiceDiscovery>().To<ConsulServiceDiscovery>().InSingletonScope();
            Bind<ITokenManager>().To<TokenManager>();
            Bind<ILogger>().To<Log4NetLogger>().InSingletonScope();
        }
    }
}