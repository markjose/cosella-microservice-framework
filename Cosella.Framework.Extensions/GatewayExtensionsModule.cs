using Cosella.Framework.Extensions.DataManagers;
using Cosella.Framework.Extensions.Interfaces;
using Ninject.Modules;

namespace Cosella.Framework.Extensions
{
    public class GatewayExtensionsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IServiceManager>().To<ServiceManager>();
        }
    }
}