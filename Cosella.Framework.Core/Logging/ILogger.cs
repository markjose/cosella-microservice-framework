using System;

namespace Cosella.Framework.Core.Logging
{
    public interface ILogger
    {
        void Debug(string message, params string[] additional);
        void Info(string message, params string[] additional);
        void Warn(string message, params string[] additional);
        void Error(string message, params string[] additional);
        void Fatal(string message, Exception ex = null, params string[] additional);
        IDisposable Metric(string message, params string[] additional);
    }
}
