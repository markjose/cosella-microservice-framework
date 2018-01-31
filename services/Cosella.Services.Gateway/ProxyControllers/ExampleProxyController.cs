using Cosella.Framework.Extensions.Authentication;
using Cosella.Framework.Extensions.Gateway;
using Ninject;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cosella.Services.Gateway.ProxyControllers
{
    [RoutePrefix("example")]
    public class ExampleProxyController : RestApiControllerWithProxy
    {
        private const string ServiceName = "Cosella.Example";

        public ExampleProxyController(IKernel kernel) : base(kernel) { }

        [HttpGet]
        [Route("streamed")]
        [Authentication("stream")]
        public async Task<IHttpActionResult> ProtectedRoute1StreamedGet()
        {
            return await ProxyStreamGet(ServiceName, 1, "example/route1");
        }

        [HttpGet]
        [Route("streamed")]
        public async Task<IHttpActionResult> Route1StreamedGet()
        {
            return await ProxyStreamGet(ServiceName, 1, "example/route1");
        }

        [HttpPost]
        [Route("streamed")]
        public async Task<IHttpActionResult> Route1StreamedPost([FromBody] ExampleData data)
        {
            return await ProxyStreamPost(ServiceName, 1, "example/route1", data);
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Route1Get()
        {
            return await ProxyRestGet<string>(ServiceName, 1, "example/route1");
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Route1Post([FromBody] ExampleData data)
        {
            return await ProxyRestPost<ExampleData, string>(ServiceName, 1, "example/route1", data);
        }

        [HttpGet]
        [Route("{routeName}")]
        public async Task<IHttpActionResult> RouteAnyGet(string routeName)
        {
            return await ProxyRestGet<string>(ServiceName, 1, $"example/{routeName}");
        }

        [HttpPost]
        [Route("{routeName}")]
        public async Task<IHttpActionResult> RouteAnyPost(string routeName, [FromBody] ExampleData data)
        {
            return await ProxyRestPost<ExampleData, string>(ServiceName, 1, $"example/{routeName}", data);
        }

        [HttpPut]
        [Route("{routeName}")]
        public async Task<IHttpActionResult> RouteAnyPut(string routeName, [FromBody] ExampleData data)
        {
            return await ProxyRestPut<ExampleData, string>(ServiceName, 1, $"example/{routeName}", data);
        }

        [HttpPatch]
        [Route("{routeName}")]
        public async Task<IHttpActionResult> RouteAnyPatch(string routeName, [FromBody] ExampleData data)
        {
            return await ProxyRestPatch<ExampleData, string>(ServiceName, 1, $"example/{routeName}", data);
        }

        [HttpDelete]
        [Route("{routeName}")]
        public async Task<IHttpActionResult> RouteAnyDelete(string routeName)
        {
            return await ProxyRestDelete<string>(ServiceName, 1, $"example/{routeName}");
        }
    }

    public class ExampleData
    {
        public string TextValue { get; set; }
    }
}
