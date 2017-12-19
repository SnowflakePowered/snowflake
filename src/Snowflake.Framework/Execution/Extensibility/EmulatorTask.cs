using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Execution.Saving;
using Snowflake.Records.Game;

namespace Snowflake.Execution.Extensibility
{
    public sealed class EmulatorTask : IEmulatorTask
    {
        public EmulatorTask(IConfigurationCollection taskConfiguration,
            IList<IInputTemplate> controllerConfiguration,
            IGameRecord emulatingGame,
            ISaveLocation gameSaveLocation)
        {
            this.TaskConfiguration = taskConfiguration;
            this.ControllerConfiguration = controllerConfiguration;
            this.EmulatingGame = emulatingGame;
            this.GameSaveLocation = gameSaveLocation;
            this.pragmas = new Dictionary<string, string>();
        }

        private readonly IDictionary<string, string> pragmas;

        public IConfigurationCollection TaskConfiguration { get; }

        public IList<IInputTemplate> ControllerConfiguration { get; }

        public IGameRecord EmulatingGame { get; }

        public ISaveLocation GameSaveLocation { get; }

        public IImmutableDictionary<string, string> Pragmas => this.pragmas.ToImmutableDictionary();

        public void AddPragma(string pragmaKey, string pragmaValue)
        {
            this.pragmas.Add(pragmaKey, pragmaValue);
        }
    }
}
