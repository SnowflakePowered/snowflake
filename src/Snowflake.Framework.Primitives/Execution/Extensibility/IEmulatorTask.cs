using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Execution.Saving;
using Snowflake.Records.Game;

namespace Snowflake.Execution.Extensibility
{
    public interface IEmulatorTask
    {
        IConfigurationCollection TaskConfiguration { get; }
        IList<IInputTemplate> ControllerConfiguration { get; }
        IGameRecord EmulatingGame { get; }
        ISaveLocation GameSaveLocation { get; }
    }
}
