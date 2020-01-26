using Snowflake.Orchestration.Saving;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Extensibility.Provisioning.Standalone;
using Snowflake.Model.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

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

        public abstract IGameEmulation ProvisionEmulationInstance(IGame game,
            IList<IEmulatedController> controllerPorts,
            string configurationProfileName);
    }
}
