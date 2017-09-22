using System.Threading;

namespace Cosella.Framework.Core.Workers
{
    public interface IInServiceWorker
    {
        void Start(CancellationToken cancellationToken);
        void Stop();
    }
}
