namespace Cosella.Services.Authenticator.Controllers
{
    using Contracts;
    using Core.Hosting;
    using Core.Interfaces;
    using JWT;
    using JWT.Algorithms;
    using JWT.Serializers;
    using log4net;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Http;

    [RoutePrefix("api/v1/users")]
    public class UserController : ApiController
    {
        private ILog _log;
        private IConfigurator _configurator;

        public UserController(ILog log, IConfigurator configurator)
        {
            _log = log;
            _configurator = configurator;
        }

        [Route("")]
        [HttpGet]
        [Roles(new[] { "superUser:users:read" })]
        public IHttpActionResult GetAll()
        {
            return Ok();
        }
    }
}