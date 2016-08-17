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
            IConfigurationCollectionStore collectionStore,
            IBiosManager biosManager,
            ISaveManager saveManager) 
            : base(appDataDirectory, collectionStore, biosManager, saveManager)
        {
        }

        public override IEmulatorInstance Instantiate(IGameRecord gameRecord, int saveSlot, IList<IEmulatedPort> ports)
        {
            throw new NotImplementedException();
        }

        [Obsolete("DEBUG ONLY")]
        public override IEmulatorInstance Instantiate(IGameRecord gameRecord, ICoreService service)
        {
            var devices = service.Get<IPluginManager>()?.Get<IInputEnumerator>()["InputEnumerator-Keyboard"].GetConnectedDevices().First();
            var contrl = service.Get<IStoneProvider>().Controllers["NES_CONTROLLER"];
            var profile = MappedControllerElementCollection.GetDefaultMappings(devices.DeviceLayout, contrl);
            var port = new EmulatedPort(0, contrl, devices, profile);
            var config = this.CollectionStore.GetConfiguration<RetroArchConfiguration>(gameRecord.Guid);
            return new RetroArchInstance(gameRecord, this, 0, service.Get<IStoneProvider>().Platforms[gameRecord.PlatformId],
                new List<IEmulatedPort>() {port},
                new Dictionary<string, IConfigurationCollection>()
                {
                    {config.FileName, config}
                });
            ;
        }
    }
}
