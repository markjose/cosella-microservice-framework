namespace Cosella.Framework.Extensions.Controllers
{
    using Core.Configuration;
    using log4net;
    using System.Web.Http;

    [RoutePrefix("")]
    public class DefaultController : ApiController
    {
        private ILog _log;
        private IConfigurator _configurator;

        public DefaultController(ILog log, IConfigurator configurator)
        {
            _log = log;
            _configurator = configurator;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("API is here");
        }
    }
}