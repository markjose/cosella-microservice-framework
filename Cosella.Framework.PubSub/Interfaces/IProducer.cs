using Cosella.Framework.PubSub.Messages;

namespace Cosella.Framework.PubSub.Interfaces
{
    public interface IProducer<T> where T : MessageBase
    {
    }
}