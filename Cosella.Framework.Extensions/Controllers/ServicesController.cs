namespace Cosella.Framework.Extensions.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Interfaces;
    using Core.Controllers;
    using Cosella.Framework.Core.Logging;

    [RoutePrefix("services")]
    public class ServicesController : RestApiController
    {
        private ILogger _log;
        private IServiceManager _serviceDataManager;

        public ServicesController(ILogger log, IServiceManager serviceDataManager)
        {
            _log = log;
            _serviceDataManager = serviceDataManager;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> List([FromUri] bool includeDescriptors)
        {
            try
            {
                return Ok(await _serviceDataManager.GetServiceDescriptions(includeDescriptors));
            }
            catch (Exception ex)
            {
                return Content(
                    HttpStatusCode.ServiceUnavailable,
                    $"Service unavailable: {ex.InnerException.Message}");
            }
        }
    }
}