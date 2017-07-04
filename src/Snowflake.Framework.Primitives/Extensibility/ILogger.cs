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
        /// The name of the logger
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
        /// <param name="messsage">The message to log</param>
        void Trace(string message);
        /// <summary>
        /// Log a message at the <see cref="LogLevel.Debug"/> log level
        /// </summary>
        /// <param name="messsage">The message to log</param>
        void Debug(string message);
        /// <summary>
        /// Log a message at the <see cref="LogLevel.Info"/> log level
        /// </summary>
        /// <param name="messsage">The message to log</param>
        void Info(string message);
        /// <summary>
        /// Log a message at the <see cref="LogLevel.Warn"/> log level
        /// </summary>
        /// <param name="messsage">The message to log</param>
        void Warn(string message);
        /// <summary>
        /// Log a message at the <see cref="LogLevel.Error"/> log level
        /// </summary>
        /// <param name="messsage">The message to log</param>
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
        /// <param name="messsage">The message to log</param>
        void Fatal(string message);
    }

    public enum LogLevel
    {
        /// <summary>
        /// The TRACE Level designates finer-grained informational events than the DEBUG 
        /// </summary>
        Trace,
        /// <summary>
        /// The DEBUG Level designates fine-grained informational events that are most useful to debug an application. 
        /// </summary>
        Debug,
        /// <summary>
        /// The INFO level designates informational messages that highlight the progress of the application at coarse-grained level. 
        /// </summary>
        Info,
        /// <summary>
        /// The WARN level designates potentially harmful situations. 
        /// </summary>
        Warn,
        /// <summary>
        /// The ERROR level designates error events that might still allow the application to continue running.
        /// </summary>
        Error,
        /// <summary>
        /// The FATAL level designates very severe error events that will presumably lead the application to abort. 
        /// </summary>
        Fatal
    }
}
