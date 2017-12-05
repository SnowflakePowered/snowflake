using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using NLog.Config;
using Snowflake.Extensibility;
using Snowflake.Services;

namespace Snowflake.Services.Logging
{
    internal class LogProvider : ILogProvider
    {
        /// <inheritdoc/>
        public Extensibility.ILogger GetLogger(string loggerName)
        {
            return new NlogLogger(loggerName);
        }
    }
}
