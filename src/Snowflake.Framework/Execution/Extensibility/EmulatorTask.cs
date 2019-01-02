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
    /// <inheritdoc/>
    public sealed class EmulatorTask : IEmulatorTask
    {
        public EmulatorTask(IGameRecord emulatingGame)
        {
            this.EmulatingGame = emulatingGame;
            this.pragmas = new Dictionary<string, string>();
            this.TaskIdentifier = Guid.NewGuid();
        }

        private readonly IDictionary<string, string> pragmas;

        /// <inheritdoc/>
        public IConfigurationCollection EmulatorConfiguration { get; set; }

        /// <inheritdoc/>
        public IList<(IInputTemplate template, IInputMapping mapping)> ControllerConfiguration { get; set; }

        /// <inheritdoc/>
        public IGameRecord EmulatingGame { get; }

        /// <inheritdoc/>
        public ISaveLocation GameSaveLocation { get; set; }

        /// <inheritdoc/>
        public IImmutableDictionary<string, string> Pragmas => this.pragmas.ToImmutableDictionary();

        /// <inheritdoc/>
        public IEmulatorTaskRoot ProcessTaskRoot { get; set; }

        /// <inheritdoc/>
        public Guid TaskIdentifier { get; }

        /// <summary>
        /// Add a pragma to the task.
        /// </summary>
        /// <param name="pragmaKey">The key of the pragma.</param>
        /// <param name="pragmaValue">The value of the pragma.</param>
        public void AddPragma(string pragmaKey, string pragmaValue)
        {
            this.pragmas.Add(pragmaKey, pragmaValue);
        }
    }
}
