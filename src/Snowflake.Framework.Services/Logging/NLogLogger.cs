using System;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Snowflake.Services.Logging
{
    internal class NlogLogger : Snowflake.Extensibility.ILogger
    {
        private readonly NLog.ILogger baseLogger;

        private static bool isSetup = false;
        private static void Setup()
        {
            if (NlogLogger.isSetup)
            {
                return;
            }
            var configuration = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget("Console");
            var asyncConsoleTarget = new AsyncTargetWrapper("Console", consoleTarget);
            consoleTarget.Layout = new NLog.Layouts.SimpleLayout("[${level:uppercase=true}] ${message}");
            configuration.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, asyncConsoleTarget);
            configuration.AddTarget(asyncConsoleTarget);
            LogManager.Configuration = configuration;
            NlogLogger.isSetup = true;
        }

        internal NlogLogger(NLog.ILogger baseLogger)
        {
            this.baseLogger = baseLogger;
        }

        public NlogLogger(string loggerName)
        {
            NlogLogger.Setup();
            this.baseLogger = LogManager.GetLogger(loggerName);
        }

        /// <inheritdoc/>
        public string Name => this.baseLogger.Name;

        /// <inheritdoc/>
        public void Debug(string message) => this.baseLogger.Debug(message);

        /// <inheritdoc/>
        public void Error(string message) => this.baseLogger.Error(message);

        /// <inheritdoc/>
        public void Error(Exception ex, string message)
        {
            this.baseLogger.Error(ex, message);
        }

        /// <inheritdoc/>
        public void Fatal(string message) => this.baseLogger.Fatal(message);

        /// <inheritdoc/>
        public void Info(string message) => this.baseLogger.Info(message);

        /// <inheritdoc/>
        public void Trace(string message) => this.baseLogger.Trace(message);

        /// <inheritdoc/>
        public void Warn(string message) => this.baseLogger.Warn(message);

        /// <inheritdoc/>
        public void Log(string messsage, Extensibility.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case Extensibility.LogLevel.Trace:
                    this.Trace(messsage);
                    break;
                case Extensibility.LogLevel.Debug:
                    this.Debug(messsage);
                    break;
                case Extensibility.LogLevel.Info:
                    this.Info(messsage);
                    break;
                case Extensibility.LogLevel.Warn:
                    this.Warn(messsage);
                    break;
                case Extensibility.LogLevel.Error:
                    this.Error(messsage);
                    break;
                case Extensibility.LogLevel.Fatal:
                    this.Fatal(messsage);
                    break;
            }
        }
    }
}
