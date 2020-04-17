using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Orchestration.Extensibility
{
    /// <summary>
    /// Describes the levels of compatibility an emulator has with a given game.
    /// </summary>
    public enum EmulatorCompatibility
    {
        /// <summary>
        /// This game is not supported by this emulator.
        /// </summary>
        Unsupported,
        /// <summary>
        /// The system files required to run this game are not available.
        /// </summary>
        MissingSystemFiles,
        /// <summary>
        /// The game has the files required to be run, but they are not in the correct format.
        /// An installation job given by <see cref="IEmulatorOrchestrator.ValidateGamePrerequisites(IGame)"/> should be 
        /// executed before running.
        /// </summary>
        RequiresValidation,
        /// <summary>
        /// This game is supported and ready to be run, requiring no validation. 
        /// </summary>
        Ready,
    }
}
