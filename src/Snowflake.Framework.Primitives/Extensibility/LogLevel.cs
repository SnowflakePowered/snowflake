namespace Snowflake.Extensibility
{
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
        Fatal,
    }
}
