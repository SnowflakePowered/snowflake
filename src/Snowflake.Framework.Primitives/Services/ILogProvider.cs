using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Extensibility;

namespace Snowflake.Services
{
    /// <summary>
    /// Provides logging services
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// Gets a new logger with the provided name.
        /// </summary>
        /// <param name="loggerName">The name of the logger.</param>
        /// <returns>A new logger instance.</returns>
        ILogger GetLogger(string loggerName);
    }
}
