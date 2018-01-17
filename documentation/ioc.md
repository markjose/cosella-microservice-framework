# Dependency Injection and Modules (IoC)

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


## Using injected services from controllers and/or long running tasks

Everything that you bind in your module you can access in controllers or in-service workers.
You should create a NinjectModule which contains all of your bindings.

### Resolving in a RestApiController or In-Service Worker

By default the ```kernel``` is passed in to the constructor of the controller or in-service worker. 
You can resolve any of the bindings that you registered in your custom modules.
You can also resolve any of the default service bindings here too. A full list of the default bindings is shown above.

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