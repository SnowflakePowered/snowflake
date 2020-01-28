using System;
using System.Collections.Generic;
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
            var asyncConsoleTarget = new AsyncTargetWrapper(consoleTarget, 2400, AsyncTargetWrapperOverflowAction.Grow)
            {
                OptimizeBufferReuse = true,
                Name = "Console"
            };
            consoleTarget.UseDefaultRowHighlightingRules = false;

            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule()
            {
                Regex = @"(?<=^\[\w+\][^w]+)\(\w+\)",
                ForegroundColor = ConsoleOutputColor.DarkGray,
            });
            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule()
            {
                Regex = @"^\[INFO\]",
                ForegroundColor = ConsoleOutputColor.DarkGreen,
            });
            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule()
            {
                Regex = @"^\[ERROR\]",
                ForegroundColor = ConsoleOutputColor.DarkYellow,
            });
            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule()
            {
                Regex = @"^\[WARN\]",
                ForegroundColor = ConsoleOutputColor.Magenta,
            });
            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule()
            {
                Regex = @"^\[FATAL\]",
                ForegroundColor = ConsoleOutputColor.Red,
            });
            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule()
            {
                Regex = @"^\[DEBUG\]",
                ForegroundColor = ConsoleOutputColor.Blue,
            });
            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule()
            {
                Regex = @"^\[TRACE\]",
                ForegroundColor = ConsoleOutputColor.Cyan,
            });
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule()
            {
                Condition = "level == LogLevel.Fatal",
                ForegroundColor = ConsoleOutputColor.DarkRed,
            });
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule()
            {
                Condition = "level == LogLevel.Warn",
                ForegroundColor = ConsoleOutputColor.DarkMagenta,
            });
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule()
            {
                Condition = "level == LogLevel.Error",
                ForegroundColor = ConsoleOutputColor.Yellow,
            });
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule()
            { 
                Condition = "level == LogLevel.Info",
                ForegroundColor = ConsoleOutputColor.Gray,
            });
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule()
            {
                Condition = "level == LogLevel.Trace",
                ForegroundColor = ConsoleOutputColor.DarkCyan,
            });
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule()
            {
                Condition = "level == LogLevel.Debug",
                ForegroundColor = ConsoleOutputColor.DarkBlue,
            });

            consoleTarget.EnableAnsiOutput = true;
            consoleTarget.Layout = new NLog.Layouts.SimpleLayout("[${level:uppercase=true}] (${logger}) ${message}");
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
