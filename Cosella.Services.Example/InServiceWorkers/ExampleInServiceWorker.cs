using System.Threading;
using Cosella.Framework.Core.Workers;

namespace Cosella.Services.Example.InServiceWorkers
{
    public class ExampleInServiceWorker : IInServiceWorker
    {
        public void Start(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}
