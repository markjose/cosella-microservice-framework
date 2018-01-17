using System;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using System.IO;
using System.Reflection;
using Cosella.Framework.Core.Hosting;
using System.Diagnostics;

using static log4net.Appender.ColoredConsoleAppender;

namespace Cosella.Framework.Core.Integrations.Log4Net
{
    public static class Log4NetAppenders
    {
        public static IAppender GetConsoleAppender()
        {
            var pattern = new PatternLayout();
            pattern.ConversionPattern = "%-10p %d %8rms - %m%n";
            pattern.ActivateOptions();

            var appender = new ColoredConsoleAppender()
            {
                Threshold = Level.All,
                Layout = pattern
            };
            appender.AddMapping(CreateMapping(Level.Alert, Colors.Yellow | Colors.HighIntensity));
            appender.AddMapping(CreateMapping(Level.Critical, Colors.Red | Colors.HighIntensity));
            appender.AddMapping(CreateMapping(Level.Debug, Colors.Cyan));
            appender.AddMapping(CreateMapping(Level.Error, Colors.Red));
            appender.AddMapping(CreateMapping(Level.Fatal, Colors.White | Colors.HighIntensity, Colors.Red | Colors.HighIntensity));
            appender.AddMapping(CreateMapping(Level.Info, Colors.White));
            appender.AddMapping(CreateMapping(Level.Verbose, Colors.Purple));
            appender.AddMapping(CreateMapping(Level.Warn, Colors.Yellow));
            appender.ActivateOptions();

            return appender;
        }

        private static LevelColors CreateMapping(Level level, Colors foreground, Colors background = 0)
        {
            var levelColor = new LevelColors();

            levelColor.Level = level;
            levelColor.ForeColor = foreground;
            levelColor.BackColor = background;
            levelColor.ActivateOptions();

            return levelColor;
        }

        internal static IAppender GetFileAppender(HostedServiceConfiguration config)
        {
            var pattern = new PatternLayout();
            pattern.ConversionPattern = "%-10p %d %8rms %-30.30c{1} %-30.30M - %m%n";
            pattern.ActivateOptions();

            var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var processId = Process.GetCurrentProcess().Id;
            var now = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");

            var appender = new RollingFileAppender();
            appender.Name = "RollingFileAppender";
            appender.Threshold = Level.All;
            appender.File = $"{directory}\\logs\\{config.ServiceName}-{now}-[{processId}].log";
            appender.AppendToFile = true;
            appender.RollingStyle = RollingFileAppender.RollingMode.Size;
            appender.MaxSizeRollBackups = 10;
            appender.MaximumFileSize = "10MB";
            appender.StaticLogFileName = true;
            appender.Layout = pattern;
            appender.ActivateOptions();

            return appender;
        }
    }
}