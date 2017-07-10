using Cosella.Framework.PubSub.Interfaces;
using Cosella.Framework.PubSub.Messages;

namespace Cosella.Framework.PubSub.Producers
{
    public abstract class Producer<T> : IProducer<T> where T : MessageBase
    {
    }
}