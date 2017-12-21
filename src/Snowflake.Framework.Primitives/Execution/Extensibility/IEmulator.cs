using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Execution.Saving;
using Snowflake.Extensibility;
using Snowflake.Platform;
using Snowflake.Records.Game;

namespace Snowflake.Execution.Extensibility
{
    /// <summary>
    /// An <see cref="IEmulator"/> handles execution of a game by creating tasks with
    /// contextual information to execute with the provided task runner.
    /// </summary>
    public interface IEmulator : IPlugin
    {
        /// <summary>
        /// Gets the task runner that this emulator plugin delegates.
        /// </summary>
        IEmulatorTaskRunner Runner { get; }

        /// <summary>
        /// Gets the emulator specific properties for this emulator.
        /// </summary>
        IEmulatorProperties Properties { get; }

        /// <summary>
        /// Creates a task to exeut
        /// </summary>
        /// <param name="executingGame"></param>
        /// <param name="saveLocation"></param>
        /// <param name="controllerConfiguration"></param>
        /// <param name="profileContext"></param>
        /// <returns></returns>
        IEmulatorTask CreateTask(IGameRecord executingGame,
            ISaveLocation saveLocation,
            IList<IEmulatedController> controllerConfiguration,
            string profileContext = "default");
    }
}
