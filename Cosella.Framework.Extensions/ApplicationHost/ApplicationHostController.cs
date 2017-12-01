using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using System.Web.Http;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    [ControllerDependsOn(typeof(IApplicationManager))]
    [RoutePrefix("applications")]
    public class ApplicationHostController : RestApiController
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult List()
        {
            return Ok();
        }
    }
}
