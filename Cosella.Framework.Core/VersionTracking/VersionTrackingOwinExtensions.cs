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
            return app.Use<RestApiVersionMiddleware>(kernel);
        }
    }
}