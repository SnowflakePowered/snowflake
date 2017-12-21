using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Execution.Saving;
using Snowflake.Extensibility;
using Snowflake.Platform;
using Snowflake.Records.Game;

namespace Snowflake.Execution.Extensibility
{
    public interface IEmulator : IPlugin
    {
        IEmulatorTaskRunner Runner { get; }

        IEmulatorProperties Properties { get; }

        IEmulatorTask CreateTask(IGameRecord executingGame,
            ISaveLocation saveLocation,
            IList<IEmulatedController> controllerConfiguration,
            string profileContext = "default");
    }
}
