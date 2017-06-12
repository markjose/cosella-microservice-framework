using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Cosella.Services.Core.Attributes
{
    public class RolesAttribute : AuthorizeAttribute
    {
        private string[] _roles;

        public new string[] Roles { get { return _roles; } }

        public RolesAttribute(string[] roles)
        {
            _roles = roles;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var user = actionContext.ControllerContext.RequestContext.Principal;
            if (user == null) return false;
            return _roles.Any(role => user.IsInRole(role));
        }
    }
}