using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Execution.Extensibility
{
    public abstract class EmulatorTaskResult : IEmulatorTaskResult
    {
        public EmulatorTaskResult(string emulatorName, DateTimeOffset startTime)
        {
            this.EmulatorName = emulatorName;
            this.StartTime = startTime;
        }

        public EmulatorTaskResult(string emulatorName)
            : this(emulatorName, DateTimeOffset.UtcNow)
        {
        }

        public string EmulatorName { get; }

        public DateTimeOffset StartTime { get; }

        public bool IsRunning { get; set; }

        public abstract void Closed();
    }
}