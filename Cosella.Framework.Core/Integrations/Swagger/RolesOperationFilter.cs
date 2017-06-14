namespace Cosella.Framework.Core.Integrations.Swagger
{
    using Attributes;
    using Swashbuckle.Swagger;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Description;

    internal class RolesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var scopes = apiDescription.ActionDescriptor.GetFilterPipeline()
                            .Select(filterInfo => filterInfo.Instance)
                            .OfType<RolesAttribute>()
                            .SelectMany(attr => attr.Roles)
                            .Distinct();

            if (scopes.Any())
            {
                if (operation.security == null)
                    operation.security = new List<IDictionary<string, IEnumerable<string>>>();

                var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
                {
                    { "cosellaRoles", scopes }
                };

                operation.security.Add(oAuthRequirements);
            }
        }
    }
}