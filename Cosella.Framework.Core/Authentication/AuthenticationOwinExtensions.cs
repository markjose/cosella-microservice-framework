using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Core.Logging;
using Ninject;
using Owin;

namespace Cosella.Framework.Core.Authentication
{
    public static class AuthenticationOwinExtensions
    {
        public static IAppBuilder UseAuthentication(this IAppBuilder app, IKernel kernel)
        {
            var log = kernel.Get<ILogger>();
            var serviceConfiguration = kernel.Get<HostedServiceConfiguration>();

            app.Use<AuthenticationMiddleware>(log, serviceConfiguration.ServiceName);

            return app;
        }
    }
}