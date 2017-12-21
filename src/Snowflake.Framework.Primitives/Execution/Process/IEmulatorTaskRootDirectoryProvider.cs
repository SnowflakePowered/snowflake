using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Execution.Extensibility;

namespace Snowflake.Execution.Process
{
    /// <summary>
    /// Provisions a folder to root instances of <see cref="IEmulatorTaskRoot"/> off of.
    /// </summary>
    public interface IEmulatorTaskRootDirectoryProvider
    {
        /// <summary>
        /// Provisions a task root directory for the given <see cref="IEmulatorTask"/>
        /// </summary>
        /// <param name="task">The task to provision a directory for.</param>
        /// <returns>A clean working directory for the <see cref="IEmulatorTaskRoot"/>.</returns>
        DirectoryInfo GetTaskRoot(IEmulatorTask task);
    }
}
