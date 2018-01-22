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
        [Route("")]
        public async Task<IHttpActionResult> Route1Get()
        {
            return await ProxyGetFor<string>(ServiceName, 1, "example/route1");
        }

        [HttpGet]
        [Route("{routeName}")]
        public async Task<IHttpActionResult> RouteAnyGet(string routeName)
        {
            return await ProxyGetFor<string>(ServiceName, 1, $"example/{routeName}");
        }

        [HttpPost]
        [Route("{routeName}")]
        public async Task<IHttpActionResult> RouteAnyPost(string routeName, [FromBody] ExampleData data)
        {
            return await ProxyPostFor<ExampleData, string>(ServiceName, 1, $"example/{routeName}", data);
        }
    }

    public class ExampleData
    {
        public string TextValue { get; set; }
    }
}
