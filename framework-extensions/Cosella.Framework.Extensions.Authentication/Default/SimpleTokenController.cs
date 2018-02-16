using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Extensions.Authentication.Interfaces;
using Ninject;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    [ControllerDependsOn(typeof(IAuthenticator), typeof(SimpleTokenController))]
    [RoutePrefix("token")]
    public class SimpleTokenController : RestApiController
    {
        private IAuthenticator _auth;
        private ITokenHandler _tokens;

        public SimpleTokenController(IKernel kernel)
        {
            _auth = kernel.Get<IAuthenticator>();
            _tokens = kernel.Get<ITokenHandler>();
        }

        [Route("")]
        [HttpPost]
        public virtual async Task<IHttpActionResult> CreateToken([FromBody] TokenRequest tokenRequest)
        {
            var user = await _auth.AuthenticateUser(tokenRequest.Identity, tokenRequest.Secret);
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
