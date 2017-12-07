using System;
using System.Collections.Generic;
using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Plugin.Emulators.TestEmulator.Configuration;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Services;

namespace Snowflake.Plugin.Emulators.TestEmulator
{
    [Plugin("TestEmulator")]
    public class TestEmulatorAdapter : EmulatorAdapter
    {
        public TestEmulatorAdapter(IPluginProvision provision,
            IStoneProvider stoneProvider,
            IConfigurationCollectionStore collectionStore,
            IBiosManager biosManager,
            ISaveManager saveManager)
            : base(provision, stoneProvider, collectionStore, biosManager, saveManager)
        {
        }

        /// <inheritdoc/>
        public override IConfigurationCollection GetConfiguration(IGameRecord gameRecord, string profileName = "default")
        {
            return this.CollectionStore.Get<ITestConfigurationCollection>(
                gameRecord.Guid, this.Name, profileName);
        }

        /// <inheritdoc/>
        public override IConfigurationCollection GetConfiguration()
        {
            return new ConfigurationCollection<ITestConfigurationCollection>();
        }

        /// <inheritdoc/>
        public override IEmulatorInstance Instantiate(IGameRecord gameRecord, IFileRecord romFile, int saveSlot, IList<IEmulatedPort> ports)
        {
            var platform = this.StoneProvider.Platforms[gameRecord.PlatformID];
            return new TestEmulatorInstance(this, gameRecord, romFile, saveSlot, platform, ports,
                this.Provision.Logger);
        }
    }
}
