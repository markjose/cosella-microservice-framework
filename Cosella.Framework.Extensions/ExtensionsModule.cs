namespace Cosella.Framework.Extensions
{
    using Ninject.Modules;
    using Interfaces;
    using DataManagers;

    public class ExtensionsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IServiceDataManager>().To<ServiceDataManager>();
        }
    }
}