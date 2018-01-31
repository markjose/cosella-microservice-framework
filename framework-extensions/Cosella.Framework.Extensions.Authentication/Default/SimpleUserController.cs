using Cosella.Framework.Core.Controllers;
using Cosella.Framework.Core.Hosting;
using Cosella.Framework.Extensions.Authentication.Interfaces;
using Ninject;
using System.Linq;
using System.Web.Http;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    [ControllerDependsOn(typeof(IUserManager), typeof(SimpleUserController))]
    [RoutePrefix("user")]
    public class SimpleUserController : RestApiController
    {
        private IUserManager _users;

        public SimpleUserController(IKernel kernel)
        {
            _users = kernel.Get<IUserManager>();
        }

        [Route("")]
        [HttpGet]
        [Authentication("user:admin")]
        public virtual IHttpActionResult ListUsers()
        {
            return Ok(_users.List().Select(u => u.Identity));
        }

        [Route("")]
        [HttpPost]
        [Authentication("user:admin")]
        public virtual IHttpActionResult AddUser([FromBody] UserRequest userRequest)
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
        [Authentication("user:admin")]
        public virtual IHttpActionResult RemoveUser(string identity)
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
        [Authentication("user")]
        public virtual IHttpActionResult GetAllRoles()
        {
            return Ok(_users.ListRoles());
        }

        [Route("{identity}/roles")]
        [HttpPatch]
        [Authentication("user:admin")]
        public virtual IHttpActionResult SetUserRoles(string identity, [FromBody] string[] roles)
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
