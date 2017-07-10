using System;

namespace Cosella.Framework.PubSub.Messages
{
    public abstract class MessageBase
    {
        private Guid _correllationId = Guid.NewGuid();
        private DateTime _created = DateTime.UtcNow;

        public Guid CorellationId { get { return _correllationId; } }
        public DateTime Created { get { return _created; } }

        public MessageBase()
        {
        }

        protected MessageBase(Guid correllationId)
        {
            _correllationId = correllationId;
        }
    }
}