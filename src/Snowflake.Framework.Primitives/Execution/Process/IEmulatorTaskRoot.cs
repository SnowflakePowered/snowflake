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
        DirectoryInfo TaskRoot { get; }
        DirectoryInfo SaveDirectory { get; }
        DirectoryInfo ConfigurationDirectory { get; }
        DirectoryInfo SystemDirectory { get; }
    }
}
