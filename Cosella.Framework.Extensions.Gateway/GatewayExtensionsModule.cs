using Ninject.Modules;

namespace Cosella.Framework.Extensions.Gateway
{
    public class GatewayExtensionsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IServiceManager>().To<ServiceManager>().InSingletonScope();
        }
    }
}