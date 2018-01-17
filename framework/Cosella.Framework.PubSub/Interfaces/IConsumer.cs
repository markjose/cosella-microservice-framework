using Cosella.Framework.PubSub.Messages;

namespace Cosella.Framework.PubSub.Interfaces
{
    public interface IConsumer<T> where T : MessageBase
    {
    }
}