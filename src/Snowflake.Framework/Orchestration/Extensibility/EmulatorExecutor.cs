using Snowflake.Orchestration.Saving;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Extensibility.Provisioning.Standalone;
using Snowflake.Model.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Snowflake.Orchestration.Extensibility
{
    public abstract class EmulatorExecutor : ProvisionedPlugin
    {
        protected EmulatorExecutor(Type pluginType)
           : this(new StandalonePluginProvision(pluginType))
        {
        }

        protected EmulatorExecutor(IPluginProvision provision)
            : base(provision)
        {
        }

        protected abstract GameEmulation ProvisionEmulationInstance(IGame game,
            IList<IEmulatedController> controllerPorts,
            string configurationProfileName);

    }
}
