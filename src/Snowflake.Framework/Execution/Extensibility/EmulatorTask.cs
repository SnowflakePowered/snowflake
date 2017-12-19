using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Execution.Process;
using Snowflake.Execution.Saving;
using Snowflake.Records.Game;

namespace Snowflake.Execution.Extensibility
{
    public sealed class EmulatorTask : IEmulatorTask
    {
        public EmulatorTask(IGameRecord emulatingGame)
        {
            this.EmulatingGame = emulatingGame;
            this.pragmas = new Dictionary<string, string>();
            this.TaskIdentifier = Guid.NewGuid();
        }

        private readonly IDictionary<string, string> pragmas;

        public IConfigurationCollection TaskConfiguration { get; set; }

        public IList<(IInputTemplate template, IInputMapping mapping)> ControllerConfiguration { get; set; }

        public IGameRecord EmulatingGame { get; }

        public ISaveLocation GameSaveLocation { get; set; }

        public IImmutableDictionary<string, string> Pragmas => this.pragmas.ToImmutableDictionary();

        public IEmulatorTaskRoot ProcessTaskRoot { get; set; }

        public Guid TaskIdentifier { get; }

        public void AddPragma(string pragmaKey, string pragmaValue)
        {
            this.pragmas.Add(pragmaKey, pragmaValue);
        }
    }
}
