using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using System.Linq;
using System.Web.Http;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    [ControllerDependsOnAttribute(typeof(IUserManager))]
    [RoutePrefix("authentication")]
    public class AuthenticationController : RestApiController
    {
        private IUserManager _users;

        public AuthenticationController(IUserManager users)
        {
            _users = users;
        }

        [Route("users")]
        [HttpGet]
        [Authentication("admin")]
        public IHttpActionResult ListUsers()
        {
            return Ok(_users.List().Select(u => u.Username));
        }

        [Route("users")]
        [HttpPost]
        [Authentication("admin")]
        public IHttpActionResult AddUser([FromBody] User userRequest)
        {
            try
            {
                return Ok(_users.Add(userRequest));
            }
            catch (UserException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("users/{username}")]
        [HttpDelete]
        [Authentication("admin")]
        public IHttpActionResult RemoveUser(string username)
        {
            try
            {
                return Ok(_users.Remove(username));
            }
            catch (UserException ex)
            {
                return ex.NotFound
                    ? (IHttpActionResult)NotFound()
                    : BadRequest(ex.Message);
            }
        }

        [Route("users/roles")]
        [HttpGet]
        [Authentication("admin")]
        public IHttpActionResult GetAllRoles()
        {
            return Ok(_users.ListRoles());
        }

        [Route("users/{username}/roles")]
        [HttpPatch]
        [Authentication("admin")]
        public IHttpActionResult SetUserRoles(string username, [FromBody] string[] roles)
        {
            try
            {
                return Ok(_users.SetRoles(username, roles));
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
