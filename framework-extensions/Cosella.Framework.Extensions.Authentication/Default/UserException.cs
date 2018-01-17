using System;
using System.Runtime.Serialization;

namespace Cosella.Framework.Extensions.Authentication.Default
{
    public class UserException : Exception
    {
        public UserException()
        {
            NotFound = true;
        }

        public UserException(string message) : base(message)
        {
        }

        public UserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public bool NotFound { get; internal set; } = false;
    }
}
