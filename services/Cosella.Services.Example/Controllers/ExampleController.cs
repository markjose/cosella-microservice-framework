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
            return Ok("GET route1");
        }

        [HttpDelete]
        [Route("route1")]
        public IHttpActionResult Route1Delete()
        {
            return Ok("DELETE route1");
        }

        [HttpPost]
        [Route("route1")]
        public IHttpActionResult Route1Post([FromBody] ExampleData data)
        {
            return Ok($"POST route1:{data.TextValue}");
        }

        [HttpPut]
        [Route("route1")]
        public IHttpActionResult Route1Put([FromBody] ExampleData data)
        {
            return Ok($"PUT route1:{data.TextValue}");
        }

        [HttpPatch]
        [Route("route1")]
        public IHttpActionResult Route1Patch([FromBody] ExampleData data)
        {
            return Ok($"PATCH route1:{data.TextValue}");
        }

        public class ExampleData
        {
            public string TextValue { get; set; }
        }
    }
}
