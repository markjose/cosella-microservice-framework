using Cosella.Framework.Core.Controllers;
using System.Web.Http;

namespace Cosella.Services.Example.Controllers
{
    [RoutePrefix("example")]
    public class ExampleController : RestApiController
    {
        [HttpGet]
        [Route("route1")]
        public IHttpActionResult Route1Get()
        {
            return Ok("route1");
        }

        [HttpPost]
        [Route("route1")]
        public IHttpActionResult Route1Post([FromBody] ExampleData data)
        {
            return Ok($"route1:{data.TextValue}");
        }

        public class ExampleData
        {
            public string TextValue { get; set; }
        }
    }
}
