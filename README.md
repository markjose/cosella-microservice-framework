# Cosella

The Cosella Microservice Framework is intended to make the development of service based solutions using .NET easier.

The framework is currently a very new project, if you want to use it, please understand:
  - there may be limited support
  - documentation will probably be incomplete
  - code may not be as tidy or organised as expected

If you like what's here already and want to improve it, PRs are most welcome.

I'll also try and be as responsive as I can to issues raised on GitHub.

## Features

#### Core Features
- RESTful API (WepApi on OWIN)
- API versioning
- API Explorer (Swagger and Swagger UI)
- Logging (Log4Net)
- Windows Console Application support
- Windows Service support (Topshelf)
- IoC/Dependency Injection (Ninject)
- Service Discovery (Consul)
- Service Scale Out (Instancing)
- Long running background tasks (InServiceWorker)

#### Core Features - In Progress 
- API usage statitics
- API version tracking
- Simple load balance

#### Extension libraries

- Cosella.Framework.Extensions.Authentication
- Cosella.Framework.Extensions.Gateway
- Cosella.Framework.Extensions.ApplicationHost

## Getting Started

#### Prerequisites

- [.NET Framework 4.7.1 targeting pack or SDK](https://www.microsoft.com/net/download/visual-studio-sdks)
- [Consul](https://www.consul.io/downloads.html) - start as local development agent ```./consul.exe agent -dev```

#### Using the framework

1. Open Visual Studio (not required, you can use VS Code or whichever IDE you normally use for .NET development)
2. Create a new 'Console App (Net Framework)' project.
3. Include the 'Cosella.Framework.Core' package from the NuGet package manager
4. Include the 'Microsoft.Owin.Host.HttpListener' package from the NuGet package manager (this is not pulled in automagically)
5. In your ```Main()``` method, create your microservice like this:
```
static int Main(string[] args)
{
	return MicroService
		.Configure(config =>
		{            
			config.ServiceName = "MyService";
			config.ServiceDisplayName = "My First Cosella Microservice";

			// If you didn't start consul, you'll need to add a port number
			// config.RestApiPort = 5000;

			// ...and disable service registration and discovery
			// config.DisableRegistration = true;
			// config.DisableServiceDiscovery = true;
		})
		.Run();
}
```
6. Choose Debug -> Start Debugging or hit F5 to start the services
7. Browse to the address shown in the console window (Typically ```http://localhost:5000```)

### Adding a RESTful API controller to the service

As the framework uses WebApi, you can add an API controller in exactly the same way as you would with a normal WebAPI project.
If you do this then all controller route mappings will be done using the MapAttributeRoutes setting. 
However, this fails to leverage the versioning, service discovery and API proxying offered by the framework.
In order to lverage this functionality you should inherit from ```RestApiController``` instead of ```ApiController```.
As with standard WebApi controllers the class needs to be ```public``` to be visible. the below controller example will appear at ```http://localhost:5000/api/v1/example/route1``` (This assumes you haven't changed any of the default options)
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

#### Alternative ```SystemRestApiController```

Inheriting from the ```SystemRestApiController``` allows you to remove the added ```api/v#``` path and place the routes in your controller at the root of the service host.
The above example would be available at ```http://localhost:5000/example/route1``` if you used the ```SystemRestApiController```.

### Adding a long running task to the service

Long running or background tasks can be added to the service by implementing the ```IInServiceWorker``` interface and then registering the service as a named service in you custom module.
```IInWorkerService``` enforces two methods, ```Start``` and ```Stop```. 
You should also make sure the ```CancellationToken``` passed to the ```Start``` method is honoured.

```
using System.Threading;
using Cosella.Framework.Core.Workers;

namespace Cosella.Services.Example.InServiceWorkers
{
    public class ExampleInServiceWorker : IInServiceWorker
    {
        public void Start(CancellationToken cancellationToken)
        {
            ...kick off your thing here and return (no long running while loop)...
        }

        public void Stop()
        {
            ...stop your thing here...
        }
    }
}
```

Then register your worker (for more information see the section on [Dependency Injection below](#ioc)). It's not essential, but good practice,
that if you register more than one ```IInServiceWorker``` binding, that you name each binding:

```
Bind<IInServiceWorker>().To<ExampleInServiceWorker>().Named("ExampleInServiceWorker");

```

### Using injected services from controllers and/or long running tasks

Everything that you bind in your module you can access in controllers or in-service workers. You should create a NinjectModule which contains all of your 

#### Resolving in a RestApiController or In-Service Worker

By default the ```kernel``` is passed in to the constructor of the controller or in-service worker. 
You can resolve any of the bindings that you registered in your custom modules.
You can also resolve any of the default service bindings here too. A full list of the default bindings is included in the [Dependency Injection](#ioc) section below.

```
[RoutePrefix("example")]
public class ExampleController : RestApiController
{
	public ExampleController(IKernel kernel) 
	{
		var logger = kernel.Get<ILogger>();
	}
}
public class ExampleInServiceWorker : IInServiceWorker
{
	public ExampleController(ILogger logger) 
	{
	}
}
```

You can use either method shown in the example above. Any resolvable types will be passed in to the constructor.
If you need to resolve by context (named bindings) you will need use the first method show above.

### Accessing other services using the ServiceRestApiClient

*TODO: I'll do this next week W/C 14-01-2018*

### All Core Configuration Options

#### Service options

In addition to ```ServiceName``` and ```ServiceDisplayName```, you can also set the following service
options. Most of these only apply when the service is hosted as a Windows Service. See the notes on the Host Container below.

| **Property Name** | **Description** | **Default Value** |
|-|-|-|
| ServiceName | The base name used to reference the Windows service in the Service Controller (each instance will have a name of ServiceName+InstanceId | Entry Assembly Name |
| ServiceDisplayName | The name which is displayed in the Windows Service manager | Entry Assemby Name |
| ServiceInstanceName | If you want to scale out you should not set this, if set all instances of the service will have this innstance name | Entry Assembly Name + InstanceId |
| ServiceDescription | A description of the Service. This is the description text shown in the Windows Service Manager | Empty String |

#### Service Discovery options

You can turn on an off certain parts of service discovery using the settings in the tbale below.

| **Property Name** | **Description** | **Default Value** |
|-|-|-|
| DisableRegistration | Turn off service registration. This service will not be registered with the service discover agent and hence not discoverable. | false |
| DisableServiceDiscovery | Turn off service enumeration and discovery by this service. This service will have no access to ther service using the ServiceRestApiClient. | false |

#### RESTful API options

You can override some of the default settings of the RESTful API. Valid properties are in the table below.

| **Property Name** | **Description** | **Default Value** |
|-|-|-|
| RestApiPort | The port number for the binding for WebApi. If not specified, free ports will automatically be handed out by the Service Discovery engine starting at 5000 | 5000 |
| RestApiVersion | The default version of endpoints, if individual endpoints are not versioned | 1 |
| RestApiHostname | The IP address or name of the binding for WebApi server | localhost |

#### Custom OWIN middleware

Your service can add your own OWIN middleware in to the middleware pipeline, by adding your OWIN IAppBuilder to the Middleware property.
More information can be found about [OWIN middleware here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/owin).
Use the delegate form ```Func<IAppBuilder, IKernel, IAppBuilder>``` for example:
```
public class MyCustomMiddleware : OwinMiddleware
{
	public MyCustomMiddleware(OwinMiddleware next, IKernel kernel) : base(next)
	{
		...kernel can be used to resolve injected bindings...
	}

	public override async Task Invoke(IOwinContext context)
	{
		...do your middleware thing here...
		await Next.Invoke(context);
	}
}
config.Middleware.Add((app, kernel) => app.Use<MyCustomMiddleware>(kernel));
```

#### Dependency Injection (IoC) <a name="ioc"></a>

Your service can add bindings to the dependency resolver by adding a ```INinjectModule```.
For more information about Ninject and Ninject Modules you can take a look at the [Ninject project site](http://www.ninject.org/).
This will make your injected bindings availabel to the resolver in your WebApi Controllers.

```
public class MyNinjectModule : NinjectModule
{
	public override void Load()
	{
		Bind<IMyFirstInterface>().To<MyFirstConcreteClass>();
		...etc...
	}
}
config.Modules.Add(MyNinjectModule);
```

The ```CoreModule``` binds access to some default implementations of common functionality:

- [```var config = kernel.Get<IConfigurator>();```](https://github.com/markjose/cosella-microservice-framework/blob/master/Cosella.Framework.Core/Configuration/IConfigurator.cs)
- [```var serviceDiscovery = kernel.Get<IServiceDiscovery>();```](https://github.com/markjose/cosella-microservice-framework/blob/master/Cosella.Framework.Client/Interfaces/IServiceDiscovery.cs)
- [```var logger = kernel.Get<ILogger>();```](https://github.com/markjose/cosella-microservice-framework/blob/master/Cosella.Framework.Core/Logging/ILogger.cs)


### A little bit about the Host Container
The default configuration for a microservice is a TopShelf console application which can be installed as a windows service using the standard TopShelf commands.
- Install: ```MyService.exe --install```
- Uninstall: ```MyService.exe --uninstall```
- Start service: ```MyService.exe --start```
- Stop service: ```MyService.exe --stop```

[More details on TopShelf can be found here.](http://topshelf-project.com/)

We are currently working on supporting Service Fabric, this should be available soon by using the following code:
```
return MicroService
    .ConfiguredFor<ServiceFabricMicroServiceContainer>(config =>
    {
        config.ServiceName = "MyService";
        config.ServiceDisplayName = "My First Cosella Microservice";
    })
    .Run();
```
