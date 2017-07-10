using Cosella.Framework.PubSub.Interfaces;
using Cosella.Framework.PubSub.Messages;

namespace Cosella.Framework.PubSub.Consumers
{
    public abstract class Consumer<T> : IConsumer<T> where T : MessageBase
    {
    }
}