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
            this.Instances = new ConcurrentDictionary<Guid, GameEmulation>();
        }

        private ConcurrentDictionary<Guid, GameEmulation> Instances { get; }

        protected abstract GameEmulation CreateEmulationInstance(IGame game,
            IList<IEmulatedController> controllerPorts,
            string configurationProfileName);

        public GameEmulation ProvisionEmulationInstance(IGame game,
            IList<IEmulatedController> controllerPorts,
            string configurationProfileName,
            SaveGame savegame)
        {
            var instance = this.CreateEmulationInstance(game, controllerPorts, configurationProfileName);
            // this should always succeed from uniqueness of GUIDs.
            this.Instances.TryAdd(instance.Guid, instance);
            return instance;
        }

        private bool disposedValue = false; // To detect redundant calls
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var instance in this.Instances.Values)
                    {
                        instance.Dispose();
                    }
                }

                disposedValue = true;
            }
        }
    }
}
