using Cosella.Framework.Core.Attributes;
using Cosella.Framework.Core.Controllers;
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
        [Roles(new [] {"private"})]
        public IHttpActionResult GetPrivate()
        {
            return Ok("private get");
        }
    }
}
