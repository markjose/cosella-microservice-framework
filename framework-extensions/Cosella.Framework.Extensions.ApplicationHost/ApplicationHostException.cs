using System;
using System.Runtime.Serialization;

namespace Cosella.Framework.Extensions.ApplicationHost
{
    [Serializable]
    internal class ApplicationHostException : Exception
    {
        public ApplicationHostException()
        {
            NotFound = true;
        }

        public ApplicationHostException(string message) : base(message)
        {
        }

        public ApplicationHostException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApplicationHostException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public bool NotFound { get; internal set; } = false;
    }
}