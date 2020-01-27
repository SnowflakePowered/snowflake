using Snowflake.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Saving;
using System.Collections.Generic;

namespace Snowflake.Orchestration.Extensibility
{
    public interface IEmulatorOrchestrator : IPlugin
    {
        IGameEmulation ProvisionEmulationInstance(IGame game, 
            IList<IEmulatedController> controllerPorts, string configurationProfileName,
            ISaveGame? initialSave);
    }
}