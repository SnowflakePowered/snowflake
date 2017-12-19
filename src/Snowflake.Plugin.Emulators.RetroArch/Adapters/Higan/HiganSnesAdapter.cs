using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Saving;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Records.Game;
using Snowflake.Services;

namespace Snowflake.Adapters.Higan
{
    public sealed class HiganSnesAdapter : ExternalEmulator
    {
        protected HiganSnesAdapter(IPluginProvision provision,
            IStoneProvider stone,
            IConfigurationCollectionStore store)
            : base(provision, stone)
        {
            this.Runner = new HiganTaskRunner();
            this.ConfigurationFactory = new HiganConfigurationFactory(provision, store);
        }

        public override IEmulatorTaskRunner Runner { get; }

        protected override IConfigurationFactory ConfigurationFactory { get; }

        public override IEmulatorTask CreateTask(IGameRecord executingGame,
            ISaveLocation saveLocation,
            IList<IEmulatedController> controllerConfiguration, string profileContext = "default")
        {
            throw new NotImplementedException();
        }

        public override IEmulatorTask CreateTask(IGameRecord executingGame, ISaveLocation saveLocation, IList<IEmulatedController> controllerConfiguration, IConfigurationCollection gameConfiguration)
        {
            throw new NotImplementedException();
        }
    }
}
