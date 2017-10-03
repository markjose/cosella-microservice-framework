using System;

namespace Cosella.Framework.Core.Authentication
{
    public class TokenManagerException : Exception
    {
        public bool IsInvalid { get; }

        public TokenManagerException(bool isInvalid, string message)
            : base(message)
        {
            IsInvalid = isInvalid;
        }
    }
}
