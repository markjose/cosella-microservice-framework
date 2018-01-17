# Adding a long running task to the service

Long running or background tasks can be added to the service by implementing the ```IInServiceWorker```
interface and registering the service as a named service in your custom module.
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

Then register your worker (for more information see the section on [Dependency Injection below](#ioc)). It's
not essential, but good practice,
that if you register more than one ```IInServiceWorker``` binding, that you name each binding:

```
Bind<IInServiceWorker>().To<ExampleInServiceWorker>().Named("ExampleInServiceWorker");

```
