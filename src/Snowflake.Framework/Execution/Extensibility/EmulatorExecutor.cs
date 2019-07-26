using Snowflake.Configuration.Input;
using Snowflake.Execution.Saving;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Execution.Extensibility
{
    public abstract class EmulatorExecutor
    {
        void ProvisionEmulationInstance(IGame game,
            IList<IEmulatedController> controllerPorts,
            string configurationProfileName,
            SaveGame savegame)
        {
            game.WithConfigurations();
        }
    }
}
