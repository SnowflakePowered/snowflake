using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Execution.Extensibility
{
    public interface IEmulatorTaskResult
    {
        string EmulatorName { get; }
        DateTimeOffset StartTime { get; }
        bool IsRunning { get; set; }
        void Closed();
    }
}
