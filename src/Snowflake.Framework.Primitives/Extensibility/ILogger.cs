using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Extensibility
{
    /// <summary>
    /// Provides logging services
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets the name of the logger
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Log a messsage with the specified <see cref="LogLevel"/>
        /// </summary>
        /// <param name="messsage">The message to log</param>
        /// <param name="logLevel">The log level</param>
        void Log(string messsage, LogLevel logLevel);

        /// <summary>
        /// Log a message at the <see cref="LogLevel.Trace"/> log level
        /// </summary>
        /// <param name="message">The message to log</param>
        void Trace(string message);

        /// <summary>
        /// Log a message at the <see cref="LogLevel.Debug"/> log level
        /// </summary>
        /// <param name="message">The message to log</param>
        void Debug(string message);

        /// <summary>
        /// Log a message at the <see cref="LogLevel.Info"/> log level
        /// </summary>
        /// <param name="message">The message to log</param>
        void Info(string message);

        /// <summary>
        /// Log a message at the <see cref="LogLevel.Warn"/> log level
        /// </summary>
        /// <param name="message">The message to log</param>
        void Warn(string message);

        /// <summary>
        /// Log a message at the <see cref="LogLevel.Error"/> log level
        /// </summary>
        /// <param name="message">The message to log</param>
        void Error(string message);

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="ex">The exception to log.</param>
        void Error(Exception ex, string message);

        /// <summary>
        /// Log a message at the <see cref="LogLevel.Fatal"/> log level
        /// </summary>
        /// <param name="message">The message to log</param>
        void Fatal(string message);
    }
}
