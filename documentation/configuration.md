# More Configuration Options

## Service options

In addition to ```ServiceName``` and ```ServiceDisplayName```, you can also set the following service
options. Most of these only apply when the service is hosted as a Windows Service. See the notes on the Host Container below.

| **Property Name** | **Description** | **Default Value** |
|-|-|-|
| ServiceName | The base name used to reference the Windows service in the Service Controller (each instance will have a name of ServiceName+InstanceId | Entry Assembly Name |
| ServiceDisplayName | The name which is displayed in the Windows Service manager | Entry Assemby Name |
| ServiceInstanceName | If you want to scale out you should not set this, if set all instances of the service will have this innstance name | Entry Assembly Name + InstanceId |
| ServiceDescription | A description of the Service. This is the description text shown in the Windows Service Manager | Empty String |

## Service Discovery options

You can turn on an off certain parts of service discovery using the settings in the tbale below.

| **Property Name** | **Description** | **Default Value** |
|-|-|-|
| DisableRegistration | Turn off service registration. This service will not be registered with the service discover agent and hence not discoverable. | false |
| DisableServiceDiscovery | Turn off service enumeration and discovery by this service. This service will have no access to ther service using the ServiceRestApiClient. | false |

## RESTful API options

You can override some of the default settings of the RESTful API. Valid properties are in the table below.

| **Property Name** | **Description** | **Default Value** |
|-|-|-|
| RestApiPort | The port number for the binding for WebApi. If not specified, free ports will automatically be handed out by the Service Discovery engine starting at 5000 | 5000 |
| RestApiVersion | The default version of endpoints, if individual endpoints are not versioned | 1 |
| RestApiHostname | The IP address or name of the binding for WebApi server | localhost |

## Custom OWIN middleware

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

## A little bit about the Host Container
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
