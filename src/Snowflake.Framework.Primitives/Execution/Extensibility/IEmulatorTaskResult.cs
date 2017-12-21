using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Execution.Extensibility
{
    /// <summary>
    /// A <see cref="IEmulatorTaskResult"/> represents the result of
    /// an <see cref="IEmulatorTask"/> once it has been processed and executed by
    /// an <see cref="IEmulatorTaskRunner"/>
    /// </summary>
    public interface IEmulatorTaskResult
    {
        /// <summary>
        /// Gets the name of the emulator that processed the task.
        /// </summary>
        string EmulatorName { get; }

        /// <summary>
        /// Gets the time this result processed.
        /// </summary>
        DateTimeOffset StartTime { get; }

        /// <summary>
        /// Gets or sets a value whether or not this task is still running.
        /// </summary>
        bool IsRunning { get; set; }

        /// <summary>
        /// A method to be invoked once this task has ended.
        /// </summary>
        void Closed();
    }
}
