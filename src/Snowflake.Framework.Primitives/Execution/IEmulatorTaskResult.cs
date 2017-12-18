using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Execution
{
    public interface IEmulatorTaskResult
    {
        string EmulatorName { get; }
        DateTimeOffset StartTime { get; }
        bool IsRunning { get; }
        Guid TaskIdentifier { get; }
        event EventHandler Closed;
    }
}
