using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Extensions.Authentication.Interfaces;
using System.Linq;
using System.Web.Http;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    [ControllerDependsOn(typeof(IUserManager), typeof(SimpleUserController))]
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
        [Authentication("User:Admin")]
        public IHttpActionResult ListUsers()
        {
            return Ok(_users.List().Select(u => u.Identity));
        }

        [Route("")]
        [HttpPost]
        [Authentication("User:Admin")]
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
        [Authentication("User:Admin")]
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
        [Authentication("User")]
        public IHttpActionResult GetAllRoles()
        {
            return Ok(_users.ListRoles());
        }

        [Route("{identity}/roles")]
        [HttpPatch]
        [Authentication("User:Admin")]
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
