using Cosella.Framework.Core;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    public static class ApplicationHostExtensions
    {
        public static MicroService AddApplicationHost(this MicroService microservice)
        {
            microservice.Configuration.Modules.Add(new ApplicationHostExtensionsModule());
            return microservice;
        }
    }
}