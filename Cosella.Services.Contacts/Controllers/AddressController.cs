namespace Cosella.Services.Contacts.Controllers
{
    using Framework.Core.Attributes;
    using Interfaces;
    using log4net;
    using Model;
    using System.Web.Http;

    [RoutePrefix("addresses")]
    public class AddressController : ApiController
    {
        private ILog _log;
        private IAddressDataManager _addressManager;

        public AddressController(ILog log, IAddressDataManager addressManager)
        {
            _log = log;
            _addressManager = addressManager;
        }

        [Route("")]
        [HttpGet]
        [Roles(new[] { "contacts:address:read" })]
        public IHttpActionResult GetAll()
        {
            return Ok(_addressManager.SummariseAll());
        }

        [Route("lookup")]
        [HttpGet]
        [Roles(new[] { "contacts:address:lookup" })]
        public IHttpActionResult Lookup([FromUri] string countryCode = "gb")
        {
            if (!_addressManager.IsSupportedCountryCode(countryCode))
            {
                return NotFound();
            }
            return Ok();
        }

        [Route("lookup")]
        [HttpPost]
        [Roles(new[] { "contacts:address:lookup" })]
        public IHttpActionResult Lookup([FromBody] AddressRequest request)
        {
            var response = _addressManager.LookupAddress(request.AreaCode, request.NameNumber, request.CountryCode);
            if (response.State == AddressResponseState.Invalid)
            {
                return BadRequest(response.InvalidMessage);
            }
            return Ok(response);
        }
    }
}