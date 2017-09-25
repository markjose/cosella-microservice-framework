using System;

namespace Cosella.Framework.Core.Logging
{
    internal class LogMetric : IDisposable
    {
        private readonly ILogger _log;
        private readonly string _message;
        private readonly string[] _additional;
        private readonly DateTime _start;

        public LogMetric(ILogger log, string message, string[] additional)
        {
            _log = log;
            _message = message;
            _additional = additional;

            _log.Info($"Started Metric: {_message}");
            _start = DateTime.UtcNow;
        }

        public void Dispose()
        {
            var time = DateTime.UtcNow - _start;
            _log.Info($"Completed Metric: {_message} on {time:HH:mm:ss.fff}");
        }
    }
}