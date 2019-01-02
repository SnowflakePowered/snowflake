using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Execution.Process;
using Snowflake.Execution.Saving;
using Snowflake.Model.Records.Game;

namespace Snowflake.Execution.Extensibility
{
    /// <summary>
    /// An emulator task represents a yet-to-be run
    /// game that will be processed and executed by an
    /// <see cref="IEmulatorTaskRunner"/> in order to run a game.
    /// </summary>
    public interface IEmulatorTask
    {
        /// <summary>
        /// Gets the configuration collection for the current emulation.
        /// </summary>
        IConfigurationCollection EmulatorConfiguration { get; }

        /// <summary>
        /// Gets a tuple of controller configuration for each template and the relative configuration mapping.
        /// </summary>
        IList<(IInputTemplate template, IInputMapping mapping)> ControllerConfiguration { get; }

        /// <summary>
        /// Gets the game that is to be emulated.
        /// </summary>
        IGameRecord EmulatingGame { get; }

        /// <summary>
        /// Gets the location where the save files for this game are to be persisted and loaded from.
        /// </summary>
        ISaveLocation GameSaveLocation { get; }

        /// <summary>
        /// Gets any string pragmas declared by the <see cref="IEmulator"/> that produced this task.
        /// </summary>
        IImmutableDictionary<string, string> Pragmas { get; }

        /// <summary>
        /// Gets the Task Root for an executable emulator, if
        /// this task requires access to an external application.
        /// <remarks>
        /// If this task does not require a task root, will be null.
        /// </remarks>
        /// </summary>
        IEmulatorTaskRoot ProcessTaskRoot { get; }

        /// <summary>
        /// Gets the identifier of this task as a unique ID.
        /// </summary>
        Guid TaskIdentifier { get; }
    }
}
