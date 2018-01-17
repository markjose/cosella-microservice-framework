# Cosella

The Cosella Microservice Framework makes the development of .NET service based solutions easier.
This framework is currently very new and undergoing regular development.
I'll also try and be as responsive as I can to issues raised on GitHub.

## Zero (tiny) Configuration Required
```
static int Main(string[] args)
{
    return MicroService
        .Configure(config =>
        {            
            config.ServiceName = "MyService";
            config.ServiceDisplayName = "My First Cosella Microservice";
            config.RestApiPort = 5000;
            config.DisableRegistration = true;
            config.DisableServiceDiscovery = true;
        })
        .Run();
}
```

Browse to [http://localhost:5000/status](http://localhost:5000/status) for status API endpoint
or [http://localhost:5000/swagger](http://localhost:5000/swagger) for interactive API explorer.

## Contents

- [Getting Started](documentation/gettingstarted.md)
- [Microservice Configuration](documentation/configuration.md)
- [Adding a REST API](documentation/restapi.md)
- [Adding a long running task](documentation/inserviceworker.md)
- [Dependency Injection and Modules](documentation/ioc.md)
- [Communication between services](documentation/restclient.md)
- [Feature List](documentation/features.md)


