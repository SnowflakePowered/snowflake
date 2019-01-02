using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Execution.Extensibility
{
    /// <inheritdoc/>s
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

        /// <inheritdoc/>
        public string EmulatorName { get; }

        /// <inheritdoc/>
        public DateTimeOffset StartTime { get; }

        /// <inheritdoc/>
        public bool IsRunning { get; set; }

        /// <inheritdoc/>
        public abstract void Closed();
    }
}
