using Snowflake.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    public interface ILogProvider
    {
        ILogger GetLogger(string loggerName);
    }
}
