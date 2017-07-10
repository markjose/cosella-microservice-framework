using Cosella.Framework.Core.Hosting;
using log4net;
using Ninject;
using Owin;

namespace Cosella.Framework.Core.VersionTracking
{
    public static class VersionTrackingOwinExtensions
    {
        public static IAppBuilder UseVerionTracking(this IAppBuilder app, IKernel kernel)
        {
            var log = kernel.Get<ILog>();
            var serviceConfiguration = kernel.Get<HostedServiceConfiguration>();

            return app.Use<RestApiVersionMiddleware>(log, serviceConfiguration.ServiceName);
        }
    }
}