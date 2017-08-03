using Snowflake.Emulator;
using System;
using Snowflake.Configuration;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using System.Collections.Generic;
using Snowflake.Extensibility.Provisioned;
using Snowflake.Services;
using Snowflake.Plugin.Emulators.TestEmulator.Configuration;
using Snowflake.Extensibility;

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

        public override IConfigurationCollection GetConfiguration(IGameRecord gameRecord, string profileName = "default")
        {
            return this.CollectionStore.Get<ITestConfigurationCollection>
                (gameRecord.Guid, this.Name, profileName);
        }

        public override IConfigurationCollection GetConfiguration()
        {
            return new ConfigurationCollection<ITestConfigurationCollection>();
        }

        public override IEmulatorInstance Instantiate(IGameRecord gameRecord, IFileRecord romFile, int saveSlot, IList<IEmulatedPort> ports)
        {
            var platform = this.StoneProvider.Platforms[gameRecord.PlatformID];
            return new TestEmulatorInstance(this, gameRecord, romFile, saveSlot, platform, ports, 
                this.Provision.Logger);
        }
    }
}
