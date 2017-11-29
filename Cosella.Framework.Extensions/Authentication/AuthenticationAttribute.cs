using Cosella.Framework.Core.Logging;
using Ninject;
using System.Linq;
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

            var user = actionContext.ControllerContext.RequestContext.Principal;

            if (user == null)
            {
                log.Warn($"Unauthenticated session failed to access protected endpoint '{actionContext.Request.RequestUri}'");
                return false;
            }

            var result = authenticator == null
                ? _roles.Any(role => user.IsInRole(role))
                : authenticator.AuthenticateInRole(user, _roles);

            if (result == false)
            {
                log.Warn($"Unauthenticated user '{user.Identity.Name}' failed to access protected endpoint '{actionContext.Request.RequestUri}'");
            }

            return result;
        }
    }
}