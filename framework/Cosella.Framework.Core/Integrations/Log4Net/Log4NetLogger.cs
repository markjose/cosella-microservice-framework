using System;
using Cosella.Framework.Core.Logging;
using log4net;
using System.Reflection;
using Cosella.Framework.Core.Hosting;
using Ninject;

namespace Cosella.Framework.Core.Integrations.Log4Net
{
    internal class Log4NetLogger : ILogger
    {
        private ILog _log;

        public Log4NetLogger(IKernel kernel, HostedServiceConfiguration configuration)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            _log = LogManager.GetLogger(entryAssembly, configuration.ServiceInstanceName);
        }

        private void Log(LogLevel level, string message, string[] additional)
        {
        }

        public void Debug(string message, params string[] additional)
        {
            Log(LogLevel.Debug, message, additional);
            _log.Debug(message);
        }

        public void Error(string message, params string[] additional)
        {
            Log(LogLevel.Error, message, additional);
            _log.Error(message);
        }

        public void Fatal(string message, Exception ex, params string[] additional)
        {
            Log(LogLevel.Fatal, message, additional);
            _log.Fatal(message);
        }

        public void Info(string message, params string[] additional)
        {
            Log(LogLevel.Info, message, additional);
            _log.Info(message);
        }

        public IDisposable Metric(string message, params string[] additional)
        {
            return new LogMetric(this, message, additional);
        }

        public void Warn(string message, params string[] additional)
        {
            Log(LogLevel.Warn, message, additional);
            _log.Warn(message);
        }
    }
}
