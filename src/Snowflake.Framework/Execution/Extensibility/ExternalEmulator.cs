using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Execution.Saving;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Records.Game;
using Snowflake.Services;

namespace Snowflake.Execution.Extensibility
{
    public abstract class ExternalEmulator : ProvisionedPlugin, IEmulator
    {
        protected ExternalEmulator(IPluginProvision provision,
            IStoneProvider stone)
            : base(provision)
        {
            this.StoneProvider = stone;
            this.Properties = new EmulatorProperties(provision, stone);
        }

        protected IStoneProvider StoneProvider { get; }

        protected abstract IConfigurationFactory ConfigurationFactory { get; }

        public abstract IEmulatorTaskRunner Runner { get; protected set; }

        public IEmulatorProperties Properties { get; }

        public abstract IEmulatorTask CreateTask(IGameRecord executingGame,
            ISaveLocation saveLocation, IList<IEmulatedController> controllerConfiguration, string profileContext = "default");

        public abstract IEmulatorTask CreateTask(IGameRecord executingGame, ISaveLocation saveLocation, IList<IEmulatedController> controllerConfiguration, IConfigurationCollection gameConfiguration);
    }
}
