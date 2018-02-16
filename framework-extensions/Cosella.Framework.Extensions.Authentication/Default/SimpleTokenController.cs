using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Extensions.Authentication.Interfaces;
using Ninject;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    [ControllerDependsOn(typeof(IUserManager), typeof(SimpleTokenController))]
    [RoutePrefix("token")]
    public class SimpleTokenController : RestApiController
    {
        private IUserManager _users;
        private ITokenHandler _tokens;

        public SimpleTokenController(IKernel kernel)
        {
            _users = kernel.Get<IUserManager>();
            _tokens = kernel.Get<ITokenHandler>();
        }

        [Route("")]
        [HttpPost]
        public virtual async Task<IHttpActionResult> CreateToken([FromBody] TokenRequest tokenRequest)
        {
            var user = _users.Authenticate(tokenRequest.Identity, tokenRequest.Secret);
            if (user == null) return Unauthorized();

            var claims = new Dictionary<string, object>()
            {
                { "identity", user.Identity },
                { "roles", user.Roles }
            };

            return Ok(await _tokens.CreateToken(claims));

        }
    }
}
