using Cosella.Framework.Core;
using System;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    public static class ApplicationHostExtensions
    {
        public static MicroService AddApplicationHost(this MicroService microservice)
        {
            microservice.Configuration.Modules.Add(new ApplicationHostExtensionsModule());
            return microservice;
        }
        public static MicroService WithApplicationHost(this MicroService microservice, Action<ApplicationHostConfiguration> configurator)
        {
            var config = new ApplicationHostConfiguration();
            configurator(config);
            microservice.Configuration.Modules.Add(new ApplicationHostExtensionsModule(config));
            return microservice;
        }
    }
}