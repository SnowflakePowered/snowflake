using Snowflake.Orchestration.Saving;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Extensibility.Provisioning.Standalone;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using Snowflake.Installation;
using Snowflake.Filesystem;
using System.Linq;

namespace Snowflake.Orchestration.Extensibility
{
    public abstract class EmulatorOrchestrator : ProvisionedPlugin, IEmulatorOrchestrator
    {
        protected EmulatorOrchestrator(Type pluginType)
           : this(new StandalonePluginProvision(pluginType))
        {
        }

        protected EmulatorOrchestrator(IPluginProvision provision)
            : base(provision)
        {
        }

        public virtual IEnumerable<ISystemFile> CheckMissingSystemFiles(IGame game)
        {
            return Enumerable.Empty<ISystemFile>();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async virtual IAsyncEnumerable<TaskResult<IFile>> ValidateGamePrerequisites(IGame game)
        {
            yield break;
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        public abstract IGameEmulation ProvisionEmulationInstance(IGame game,
            IList<IEmulatedController> controllerPorts,
            string configurationProfileName,
            ISaveGame? initialSave);
    }
}
