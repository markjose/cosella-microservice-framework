using System;

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

        public bool NotFound { get; internal set; } = false;
    }
}
