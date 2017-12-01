using Ninject.Modules;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    public class ApplicationHostExtensionsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IApplicationManager>().To<ApplicationManager>();
        }
    }
}