namespace Cosella.Framework.Extensions.Controllers
{
    using log4net;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Interfaces;
    using Core.Controllers;

    [RoutePrefix("services")]
    public class ServicesController : RestApiController
    {
        private ILog _log;
        private IServiceDataManager _serviceDataManager;

        public ServicesController(ILog log, IServiceDataManager serviceDataManager)
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