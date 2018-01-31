using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Extensions.Authentication.Interfaces;
using System.Linq;
using System.Web.Http;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    [ControllerDependsOn(typeof(IUserManager))]
    [RoutePrefix("user")]
    public class SimpleUserController : RestApiController
    {
        private IUserManager _users;

        public SimpleUserController(IUserManager users)
        {
            _users = users;
        }

        [Route("")]
        [HttpGet]
        [Authentication("Authentication:Admin")]
        public IHttpActionResult ListUsers()
        {
            return Ok(_users.List().Select(u => u.Identity));
        }

        [Route("")]
        [HttpPost]
        [Authentication("Authentication:Admin")]
        public IHttpActionResult AddUser([FromBody] UserRequest userRequest)
        {
            try
            {
                return Ok(_users.Add(
                    userRequest.Identity,
                    userRequest.Secret,
                    userRequest.Roles));
            }
            catch (UserException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{identity}")]
        [HttpDelete]
        [Authentication("Authentication:Admin")]
        public IHttpActionResult RemoveUser(string identity)
        {
            try
            {
                return Ok(_users.Remove(identity));
            }
            catch (UserException ex)
            {
                return ex.NotFound
                    ? (IHttpActionResult)NotFound()
                    : BadRequest(ex.Message);
            }
        }

        [Route("roles")]
        [HttpGet]
        [Authentication("Authentication")]
        public IHttpActionResult GetAllRoles()
        {
            return Ok(_users.ListRoles());
        }

        [Route("{identity}/roles")]
        [HttpPatch]
        [Authentication("Authentication:Admin")]
        public IHttpActionResult SetUserRoles(string identity, [FromBody] string[] roles)
        {
            try
            {
                return Ok(_users.SetRoles(identity, roles));
            }
            catch (UserException ex)
            {
                return ex.NotFound
                    ? (IHttpActionResult)NotFound()
                    : BadRequest(ex.Message);
            }
        }
    }
}
