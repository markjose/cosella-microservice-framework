using Cosella.Framework.Core.Logging;
using Cosella.Framework.Extensions.Authentication.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Cosella.Framework.Extensions.Authentication
{
    public class AuthenticationAttribute : AuthorizeAttribute
    {
        private string[] _roles;

        public new string[] Roles { get { return _roles; } }

        public AuthenticationAttribute(params string[] roles)
        {
            _roles = roles;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var kernel = actionContext
                .RequestContext
                .Configuration
                .DependencyResolver;

            var log = (ILogger)kernel.GetService(typeof(ILogger));
            var authenticator = (IAuthenticator)kernel.GetService(typeof(IAuthenticator));

            var principle = actionContext.ControllerContext.RequestContext.Principal;
            if (principle == null)
            {
                log.Warn($"Unauthenticated session failed to access protected endpoint '{actionContext.Request.RequestUri}'");
                return false;
            }

            var user = authenticator.UserFromIdentity(principle.Identity.Name).Result;
            var result = authenticator == null
                ? _roles.Any(role => user.IsInRole(role))
                : authenticator.AuthenticateInRole(user, _roles).Result;

            if (result == false)
            {
                log.Warn($"Unauthenticated user '{user.Identity}' failed to access protected endpoint '{actionContext.Request.RequestUri}'");
            }

            return result;
        }
    }
}