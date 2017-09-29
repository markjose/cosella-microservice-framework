using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Core.Logging;
using Ninject;
using Owin;

namespace Cosella.Framework.Core.VersionTracking
{
    public static class VersionTrackingOwinExtensions
    {
        public static IAppBuilder UseVersionTracking(this IAppBuilder app, IKernel kernel)
        {
            var log = kernel.Get<ILogger>();
            var serviceConfiguration = kernel.Get<HostedServiceConfiguration>();

            return app.Use<RestApiVersionMiddleware>(log, serviceConfiguration.ServiceName);
        }
    }
}