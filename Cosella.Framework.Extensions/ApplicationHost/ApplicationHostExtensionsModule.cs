using Ninject.Modules;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    public class ApplicationHostExtensionsModule : NinjectModule
    {
        private readonly ApplicationHostConfiguration _config;

        public ApplicationHostExtensionsModule()
        {

        }
        public ApplicationHostExtensionsModule(ApplicationHostConfiguration config)
        {
            _config = config;
        }

        public override void Load()
        {
            Bind<IApplicationManager>().To<ApplicationManager>().InSingletonScope();
            Bind<ApplicationHostConfiguration>().ToConstant(_config);

            if(_config != null && _config.EnableApplicationManagerApi)
            {
                Bind<IApplicationManagerOverRest>();
            }
        }
    }
}