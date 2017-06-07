﻿namespace Cosella.Services.Extensions
{
    using Ninject.Modules;
    using Interfaces;
    using DataManagers;

    public class GatewayModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IServiceDataManager>().To<ServiceDataManager>();
        }
    }
}