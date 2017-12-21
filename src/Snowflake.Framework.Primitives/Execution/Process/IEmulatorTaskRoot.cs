using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Execution.Process
{
    /// <summary>
    /// The directory for an emulator process.
    /// </summary>
    public interface IEmulatorTaskRoot
    {
        /// <summary>
        /// Gets the root directory for the task that involves an external emulator executable.
        /// </summary>
        DirectoryInfo TaskRoot { get; }

        /// <summary>
        /// Gets the directory that the external emulator executable will save game data in.
        /// </summary>
        DirectoryInfo SaveDirectory { get; }

        /// <summary>
        /// Gets the directory that the external emulator executable will load configuration data from.
        /// </summary>
        DirectoryInfo ConfigurationDirectory { get; }

        /// <summary>
        /// Gets the directory that the external emulator executable will load system or BIOS files from.
        /// </summary>
        DirectoryInfo SystemDirectory { get; }
    }
}
