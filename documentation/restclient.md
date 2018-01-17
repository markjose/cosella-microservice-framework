# Accessing other services

## The ServiceRestApiClient

You can access any of the RESTful endpoints of any of the services using an HTTP client, but this means you
will need to do all of the service discovery yourself. If you don't want to worry about this and leverage the
simple load balancer included in the framework you can use the ```ServiceRestApiClient``` class.

You'll need to install the [Cosella.Framework.Client](https://www.nuget.org/packages/Cosella.Framework.Client/)
package from Nuget. Then you can reference services by name or instance name rather than worrying about where
those services are deployed. Services which have the service registration option disabled will not be available
using the client. Services that have service discovery disabled will not be able to use the client either.

You can call a service endpoint by creating a client instance and then calling the relative endpoint as shown in
the example below:

```
var client = new ServiceRestApiClient("MyService", kernel.Get<IServiceDiscovery>());
...or...
var client = kernel.Get<ServiceRestApiClient>(new ConstructorArgument("serviceName", "MyService"));
...or...
var client = kernel.Get<IServiceRestClientFactory>().Create("MyService"); // <- coming soon

var myServiceRestResponse = client.Get<MyPayloadType>("example/route1");
```