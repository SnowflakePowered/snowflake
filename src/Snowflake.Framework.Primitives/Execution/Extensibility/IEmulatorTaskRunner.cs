using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Execution.Extensibility
{
    /// <summary>
    /// An <see cref="IEmulatorTaskRunner"/> proccesses and runs <see cref="IEmulatorTask"/> instances.
    /// </summary>
    public interface IEmulatorTaskRunner
    {
        /// <summary>
        /// Executes the given <see cref="IEmulatorTask"/> asynchronously,
        /// generating and serializing configuration files, persisting 
        /// and loading save files, and other general preparation.
        /// </summary>
        /// <param name="task">The task to run.</param>
        /// <returns>The result of the executed task.</returns>
        Task<IEmulatorTaskResult> ExecuteEmulationAsync(IEmulatorTask task);
    }
}
