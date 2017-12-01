using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using System.Web.Http;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    [ControllerDependsOn(typeof(IApplicationManager))]
    [RoutePrefix("")]
    public class _DefaultController : SystemRestApiController
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult Default([FromUri] string tenant = null)
        {
            return Ok(tenant ?? "_root");
        }
        [Route("{tenant}")]
        [HttpGet]
        public IHttpActionResult TenantDefault(string tenant)
        {
            return Ok(tenant ?? "_root");
        }
    }
}
