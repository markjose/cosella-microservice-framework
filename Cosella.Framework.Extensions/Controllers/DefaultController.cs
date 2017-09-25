namespace Cosella.Framework.Extensions.Controllers
{
    using Core.Configuration;
    using Core.Controllers;
    using Cosella.Framework.Core.Logging;
    using System.Web.Http;

    [RoutePrefix("")]
    public class DefaultController : RestApiController
    {
        private ILogger _log;
        private IConfigurator _configurator;

        public DefaultController(ILogger log, IConfigurator configurator)
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