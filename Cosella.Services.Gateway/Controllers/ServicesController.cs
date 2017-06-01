namespace Cosella.Services.Gateway.Controllers
{
    using Core.Interfaces;
    using log4net;
    using Newtonsoft.Json;
    using System.Web.Http;

    [RoutePrefix("services")]
    public class ServicesController : ApiController
    {
        private ILog _log;
        private IServiceDiscovery _discovery;

        public ServicesController(ILog log, IServiceDiscovery discovery)
        {
            _log = log;
            _discovery = discovery;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult List()
        {
            var services = _discovery.ListServices().Result;
            return Ok(JsonConvert.SerializeObject(services));
        }
    }
}