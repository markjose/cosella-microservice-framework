using Ninject.Modules;

namespace Cosella.Framework.Extensions.Gateway
{
    public class GatewayExtensionsModule : NinjectModule
    {
        private GatewayConfiguration _configuration;

        public GatewayExtensionsModule(GatewayConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void Load()
        {
            if (_configuration.DisableServiceManager == false)
            {
                Bind<IServiceManager>().To<ServiceManager>().InSingletonScope();
            }
        }
    }
}