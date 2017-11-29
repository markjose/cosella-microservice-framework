using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Logging;
using Cosella.Framework.Extensions.Authentication;
using Cosella.Services.Contacts.Interfaces;
using Cosella.Services.Contacts.Model;
using System.Web.Http;

namespace Cosella.Services.Contacts.Controllers
{
    [RoutePrefix("addresses")]
    public class AddressController : RestApiController
    {
        private ILogger _log;
        private IAddressDataManager _addressManager;

        public AddressController(ILogger log, IAddressDataManager addressManager)
        {
            _log = log;
            _addressManager = addressManager;
        }

        [Route("")]
        [HttpGet]
        [Authentication("contacts:address:read")]
        public IHttpActionResult GetAll()
        {
            return Ok(_addressManager.SummariseAll());
        }

        [Route("lookup")]
        [HttpGet]
        [Authentication("contacts:address:lookup")]
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
        [Authentication("contacts:address:lookup")]
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