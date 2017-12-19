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
    public interface IEmulatorTask
    {
        IConfigurationCollection TaskConfiguration { get; }
        IList<(IInputTemplate template, IInputMapping mapping)> ControllerConfiguration { get; }
        IGameRecord EmulatingGame { get; }
        ISaveLocation GameSaveLocation { get; }
        IImmutableDictionary<string, string> Pragmas { get; }
        IEmulatorTaskRoot ProcessTaskRoot { get; }
        Guid TaskIdentifier { get; }
    }
}
