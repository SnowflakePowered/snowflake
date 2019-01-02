using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Execution.Saving;
using Snowflake.Extensibility;
using Snowflake.Model.Records.Game;
using Snowflake.Platform;

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
        /// Gets the factory that produces configuration files for this emulator.
        /// </summary>
        IConfigurationFactory ConfigurationFactory { get; }

        /// <summary>
        /// Creates a task to execute the given game with the emulator <see cref="Runner"/>
        /// </summary>
        /// <param name="executingGame">The game to execute.</param>
        /// <param name="saveLocation">The provided save location.</param>
        /// <param name="controllerConfiguration">The emulated controller configuration.</param>
        /// <param name="configurationProfile">The profile name of the emulator configuration.</param>
        /// <returns></returns>
        IEmulatorTask CreateTask(IGameRecord executingGame,
            ISaveLocation saveLocation,
            IList<IEmulatedController> controllerConfiguration,
            string configurationProfile = "default");
    }
}
