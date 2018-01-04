using System;
using System.Runtime.Serialization;

namespace Cosella.Framework.Extensions.Gateway
{
    [Serializable]
    internal class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(string serviceName) : base($"The '{serviceName}' service was not found or available.")
        {
        }
    }
}