using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Execution.Extensibility;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Higan.Configuration;
using Snowflake.Plugin.Emulators.RetroArch.Input;
using Snowflake.Records.Game;

namespace Snowflake.Adapters.Higan
{
    public sealed class HiganConfigurationFactory : ConfigurationFactory<HiganRetroArchConfiguration, RetroPadTemplate>
    {
        private IConfigurationCollectionStore CollectionStore { get; }

        internal HiganConfigurationFactory(IPluginProvision provision,
            IConfigurationCollectionStore collectionStore)
            : base(provision)
        {
            this.CollectionStore = collectionStore;
        }

        internal HiganConfigurationFactory(IEnumerable<IInputMapping> inputMappings,
            IConfigurationCollectionStore collectionStore)
            : base(inputMappings)
        {
            this.CollectionStore = collectionStore;
        }

        public override IConfigurationCollection<HiganRetroArchConfiguration> GetConfiguration(IGameRecord gameRecord, string profileName = "default")
        {
            return this.CollectionStore.Get<HiganRetroArchConfiguration>(gameRecord.Guid, "retroarch-higan", profileName);
        }

        public override IConfigurationCollection<HiganRetroArchConfiguration> GetConfiguration()
        {
            return new ConfigurationCollection<HiganRetroArchConfiguration>();
        }

        public override IInputTemplate<RetroPadTemplate> GetInputTemplate(IEmulatedController emulatedDevice)
        {
            return new InputTemplate<RetroPadTemplate>(emulatedDevice.LayoutMapping, emulatedDevice.PortIndex);
        }
    }
}
