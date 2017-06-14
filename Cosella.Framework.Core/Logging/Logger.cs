using System;
using log4net.Core;

namespace Cosella.Framework.Core.Logging
{
    public class Logger : ILog
    {
        private log4net.ILog _internalLogger;

        public Logger(log4net.ILog internalLogger)
        {
            _internalLogger = internalLogger;
        }

        public bool IsDebugEnabled => _internalLogger.IsDebugEnabled;
        public bool IsErrorEnabled => _internalLogger.IsErrorEnabled;
        public bool IsFatalEnabled => _internalLogger.IsFatalEnabled;
        public bool IsInfoEnabled => _internalLogger.IsInfoEnabled;
        public bool IsWarnEnabled => _internalLogger.IsWarnEnabled;

        ILogger ILoggerWrapper.Logger => _internalLogger.Logger;

        public void Debug(object message) => _internalLogger.Debug(message);

        public void Debug(object message, Exception exception) => _internalLogger.Debug(message, exception);

        public void DebugFormat(string format, object arg0) => _internalLogger.DebugFormat(format, arg0);

        public void DebugFormat(string format, params object[] args) => _internalLogger.DebugFormat(format, args);

        public void DebugFormat(IFormatProvider provider, string format, params object[] args) => _internalLogger.DebugFormat(provider, format, args);

        public void DebugFormat(string format, object arg0, object arg1) => _internalLogger.DebugFormat(format, arg0, arg1);

        public void DebugFormat(string format, object arg0, object arg1, object arg2) => _internalLogger.DebugFormat(format, arg0, arg1, arg2);

        public void Error(object message) => _internalLogger.Error(message);

        public void Error(object message, Exception exception) => _internalLogger.Error(message, exception);

        public void ErrorFormat(string format, object arg0) => _internalLogger.ErrorFormat(format, arg0);

        public void ErrorFormat(string format, params object[] args) => _internalLogger.ErrorFormat(format, args);

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args) => _internalLogger.ErrorFormat(provider, format, args);

        public void ErrorFormat(string format, object arg0, object arg1) => _internalLogger.ErrorFormat(format, arg0, arg1);

        public void ErrorFormat(string format, object arg0, object arg1, object arg2) => _internalLogger.ErrorFormat(format, arg0, arg1, arg2);

        public void Fatal(object message) => _internalLogger.Fatal(message);

        public void Fatal(object message, Exception exception) => _internalLogger.Fatal(message, exception);

        public void FatalFormat(string format, object arg0) => _internalLogger.FatalFormat(format, arg0);

        public void FatalFormat(string format, params object[] args) => _internalLogger.FatalFormat(format, args);

        public void FatalFormat(IFormatProvider provider, string format, params object[] args) => _internalLogger.FatalFormat(provider, format, args);

        public void FatalFormat(string format, object arg0, object arg1) => _internalLogger.FatalFormat(format, arg0, arg1);

        public void FatalFormat(string format, object arg0, object arg1, object arg2) => _internalLogger.FatalFormat(format, arg0, arg1, arg2);

        public void Info(object message) => _internalLogger.Info(message);

        public void Info(object message, Exception exception) => _internalLogger.Info(message, exception);

        public void InfoFormat(string format, object arg0) => _internalLogger.InfoFormat(format, arg0);

        public void InfoFormat(string format, params object[] args) => _internalLogger.InfoFormat(format, args);

        public void InfoFormat(IFormatProvider provider, string format, params object[] args) => _internalLogger.InfoFormat(provider, format, args);

        public void InfoFormat(string format, object arg0, object arg1) => _internalLogger.InfoFormat(format, arg0, arg1);

        public void InfoFormat(string format, object arg0, object arg1, object arg2) => _internalLogger.InfoFormat(format, arg0, arg1, arg2);

        public void Warn(object message) => _internalLogger.Warn(message);

        public void Warn(object message, Exception exception) => _internalLogger.Warn(message, exception);

        public void WarnFormat(string format, object arg0) => _internalLogger.WarnFormat(format, arg0);

        public void WarnFormat(string format, params object[] args) => _internalLogger.WarnFormat(format, args);

        public void WarnFormat(IFormatProvider provider, string format, params object[] args) => _internalLogger.WarnFormat(provider, format, args);

        public void WarnFormat(string format, object arg0, object arg1) => _internalLogger.WarnFormat(format, arg0, arg1);

        public void WarnFormat(string format, object arg0, object arg1, object arg2) => _internalLogger.WarnFormat(format, arg0, arg1, arg2);
    }
}