using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Extensibility;
using NLog;
using NLog.Config;

namespace Snowflake.Services.Logging
{
    internal class LogProvider : ILogProvider
    {
        public Extensibility.ILogger GetLogger(string loggerName)
        {
            return new NlogLogger(loggerName);
        }
    }
}
