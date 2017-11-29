using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Extensions.Authentication;
using System.Web.Http;

namespace Cosella.Services.Gateway.Controllers
{
    [RoutePrefix("test")]
    public class TestController: RestApiController
    {
        [Route("public")]
        [HttpGet]
        public IHttpActionResult GetPublic()
        {
            return Ok("public get");
        }
        [Route("private")]
        [HttpGet]
        [Authentication("private")]
        public IHttpActionResult GetPrivate()
        {
            return Ok("private get");
        }
        [Route("granular")]
        [HttpGet]
        [Authentication("private", "granular")]
        public IHttpActionResult GetPrivateGranular()
        {
            return Ok("private granular get");
        }
    }
}
