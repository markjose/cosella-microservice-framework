using Cosella.Framework.Core.Controllers;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Cosella.Framework.Core.Hosting
{

    public class RoutePrefixProvider : DefaultDirectRouteProvider
    {
        private readonly HostedServiceConfiguration _serviceConfig;

        public RoutePrefixProvider(HostedServiceConfiguration serviceConfig)
        {
            _serviceConfig = serviceConfig;
        }

        protected override string GetRoutePrefix(HttpControllerDescriptor controllerDescriptor)
        {
            var existingPrefix = base.GetRoutePrefix(controllerDescriptor);
            if (typeof(SystemRestApiController).IsAssignableFrom(controllerDescriptor.ControllerType))
            {
                return existingPrefix;
            }
            else if (typeof(RestApiController).IsAssignableFrom(controllerDescriptor.ControllerType))
            {
                return $"api/v{_serviceConfig.RestApiVersion}/{existingPrefix}";
            }
            return $"api/{existingPrefix}";
        }
    }
}