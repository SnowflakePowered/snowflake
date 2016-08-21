using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Emulator;
using Snowflake.Extensibility;
using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Platform;
using Snowflake.Records.Game;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters
{
    [Plugin("RetroArchCommon")]
    public class RetroArchCommonAdapter : Emulator.EmulatorAdapter
    {
        public RetroArchCommonAdapter(string appDataDirectory, 
            IStoneProvider stoneProvider,
            IConfigurationCollectionStore collectionStore,
            IBiosManager biosManager,
            ISaveManager saveManager) 
            : base(appDataDirectory, stoneProvider, collectionStore, biosManager, saveManager)
        {
        }

        public override IEmulatorInstance Instantiate(IGameRecord gameRecord, int saveSlot, IList<IEmulatedPort> ports)
        {
            var retroarchConfig =
                this.CollectionStore.GetConfiguration<RetroArchConfiguration>(gameRecord.Guid);
            var configurations = new Dictionary<string, IConfigurationCollection>
            {
                {retroarchConfig.FileName, retroarchConfig}
            
            };
            var platform = this.StoneProvider.Platforms[gameRecord.PlatformId];

            return new RetroArchInstance(gameRecord, gameRecord.Files.First(), this, saveSlot, platform, ports, configurations);
        }

        [Obsolete("DEBUG ONLY")]
        public override IEmulatorInstance Instantiate(IGameRecord gameRecord, ICoreService service)
        {
            var devices = service.Get<IPluginManager>()?.Get<IInputEnumerator>()["InputEnumerator-Keyboard"].GetConnectedDevices().First();
            var contrl = service.Get<IStoneProvider>().Controllers["NES_CONTROLLER"];
            var profile = MappedControllerElementCollection.GetDefaultMappings(devices.DeviceLayout, contrl);
            var port = new EmulatedPort(0, contrl, devices, profile);
            return this.Instantiate(gameRecord, 0, new List<IEmulatedPort> {port});
        }
    }
}
