# Adding a RESTful API controller to the service

As the framework uses WebApi, you can add an API controller in exactly the same way as you would with a normal
WebAPI project. If you do this, then all controller routes will be mapped using the MapAttributeRoutes setting. 
However, this fails to leverage the versioning, service discovery and API proxying offered by the framework.
In order to leverage this functionality you should inherit from ```RestApiController``` instead of
```ApiController```. As with standard WebApi controllers the class needs to be ```public``` to be visible.
The below controller example will appear at ```http://localhost:5000/api/v1/example/route1``` (This
assumes you haven't changed any of the default options)

```
using Cosella.Framework.Core.Controllers;
using System.Web.Http;

namespace MyProject
{
    [RoutePrefix("example")]
    public class ExampleController : RestApiController
    {
        [HttpGet]
        [Route("route1")]
        public IHttpActionResult Route1Get()
        {
            return Ok("route1");
        }
    }
}
```

### Alternative ```SystemRestApiController```

Inheriting from the ```SystemRestApiController``` allows you to remove the added ```api/v#``` part of the
path and map the routes in your controller to the root of the service host.
The above example would be available at ```http://localhost:5000/example/route1``` if you used the
```SystemRestApiController```.