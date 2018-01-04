using Cosella.Framework.Core.Integrations.Swagger;
using Cosella.Framework.Core.VersionTracking;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using Swashbuckle.Application;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Cosella.Framework.Core.Hosting
{
    internal class Startup
    {
        private IKernel _kernel;

        public Startup(IKernel kernel)
        {
            _kernel = kernel;
        }

        internal void Configuration(IAppBuilder app)
        {
            var serviceConfiguration = _kernel.Get<HostedServiceConfiguration>();
            var config = new HttpConfiguration();

            config.Services.Replace(typeof(IHttpControllerTypeResolver), new CustomControllerResolver(_kernel));

            config.MapHttpAttributeRoutes(new RoutePrefixProvider(serviceConfiguration));

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            config.Formatters.Add(config.Formatters.JsonFormatter);

            config
                .EnableSwagger(swaggerConfig =>
                {
                    swaggerConfig.SingleApiVersion(
                        $"v{serviceConfiguration.RestApiVersion}",
                        $"{serviceConfiguration.ServiceName} API");

                    swaggerConfig.ApiKey("apiKey")
                        .Description("API Key Authentication")
                        .Name("X-ApiKey")
                        .In("header");

                    swaggerConfig.OperationFilter<RolesOperationFilter>();
                })
                .EnableSwaggerUi(swaggerUiConfig => {
                    swaggerUiConfig.EnableApiKeySupport("X-ApiKey", "header");
                });

            app
                .UseNinjectMiddleware(() => _kernel)
                .UseCors(CorsOptions.AllowAll)
                .UseVersionTracking(_kernel);

            serviceConfiguration
                .Middleware
                .ForEach(loader => loader(app, _kernel));

            app.UseNinjectWebApi(config);
        }
    }
}