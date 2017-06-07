using Cosella.Services.Core.Integrations.Swagger;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using Swashbuckle.Application;
using System.Web.Http;

namespace Cosella.Services.Core.Hosting
{
    public class Startup
    {
        private IKernel _kernel;

        public Startup(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Configuration(IAppBuilder app)
        {
            var serviceConfiguration = _kernel.Get<HostedServiceConfiguration>();
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);
            //config.EnableSystemDiagnosticsTracing();

            config
                .EnableSwagger(swaggerConfig =>
                {
                    swaggerConfig.SingleApiVersion(
                        $"v{serviceConfiguration.RestApiVersion}",
                        $"{serviceConfiguration.ServiceName} API");

                    swaggerConfig.OperationFilter<RolesOperationFilter>();
                })
                .EnableSwaggerUi();

            app
                .UseNinjectMiddleware(() => _kernel)
                .UseNinjectWebApi(config);
        }
    }
}