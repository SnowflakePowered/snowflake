using Snowflake.Extensibility;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Saving;
using Snowflake.Orchestration.SystemFiles;
using System.Collections.Generic;

namespace Snowflake.Orchestration.Extensibility
{
    /// <summary>
    /// An <see cref="IEmulatorOrchestrator"/> is responsible for orchestrating the emulator, or emulation of a given game.
    /// This may be done through manipulation of configuration files and command line arguments, or by directly running the game,
    /// but this is abstracted using the provisioning of <see cref="IGameEmulation"/> instances.
    /// </summary>
    public interface IEmulatorOrchestrator : IPlugin
    {
        /// <summary>
        /// Gets a list of BIOS files that are missing from the <see cref="ISystemFileProvider"/> service that
        /// are necessary to run the given game.
        /// If any of these are missing, then execution SHOULD NOT continue.
        /// </summary>
        /// <param name="game">The game that is to be run.</param>
        /// <returns>A list of BIOS files that are missing that are required to run the given game.</returns>
        IEnumerable<ISystemFile> CheckMissingSystemFiles(IGame game);
        /// <summary>
        /// Validates game prerequisites, and transforms if necessary, any files registered with
        /// the game into a format usable for this particular <see cref="IEmulatorOrchestrator"/>.
        /// 
        /// If such format already exists, do nothing.
        /// </summary>
        /// <param name="game">The game that is to be run.</param>
        /// <returns>A queuable installation job that transforms the files of the game into a format usable for
        /// this particular <see cref="IEmulatorOrchestrator"/>.</returns>
        IAsyncEnumerable<TaskResult<IFile>> ValidateGamePrerequisites(IGame game);
        /// <summary>
        /// Provision the emulation instance to run the game.
        /// </summary>
        /// <param name="game">The game to be run.</param>
        /// <param name="controllerPorts">The input devices to be used during the emulation.</param>
        /// <param name="configurationProfileName">The name of the configuration profile used in this emulation.</param>
        /// <param name="initialSave">The initial save, if any, to resume the emulation from.</param>
        /// <returns>An emulator specific <see cref="IGameEmulation"/> instance that can be used to begin the emulation.</returns>
        IGameEmulation ProvisionEmulationInstance(IGame game, 
            IList<IEmulatedController> controllerPorts, string configurationProfileName,
            ISaveGame? initialSave);
    }
}