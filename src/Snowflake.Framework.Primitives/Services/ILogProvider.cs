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
        ILogger GetLogger(string loggerName);
    }
}
