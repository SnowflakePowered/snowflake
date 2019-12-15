using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Orchestration.Process
{
    /// <summary>
    /// Represents an external emulator that can be called using the operating system shell.
    /// </summary>
    public interface IEmulatorExecutable
    {
        /// <summary>
        /// Gets the name of the emulator.
        /// </summary>
        string EmulatorName { get; }

        /// <summary>
        /// Gets the version of the emulator.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Gets a <see cref="IProcessBuilder"/> that can be used to start this emulator.
        /// </summary>
        /// <returns>A <see cref="IProcessBuilder"/> that can be used to start this emulator.</returns>
        IProcessBuilder GetProcessBuilder();
    }
}
