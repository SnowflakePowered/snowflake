using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Hotkey;
using Snowflake.Configuration.Input;
using Snowflake.Emulator;
using Snowflake.Extensibility;
using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Platform;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Executable;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Input.Hotkeys;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Shaders;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters
{
    public abstract class RetroArchCommonAdapter : Emulator.EmulatorAdapter
    {
        protected RetroArchProcessHandler ProcessHandler { get; }
        protected ShaderManager ShaderManager { get; }
        public string CorePath { get; }

        protected RetroArchCommonAdapter(string appDataDirectory, 
            RetroArchProcessHandler processHandler,
            IStoneProvider stoneProvider,
            IConfigurationCollectionStore collectionStore,
            IHotkeyTemplateStore hotkeyStore,
            IBiosManager biosManager,
            ISaveManager saveManager, 
            ShaderManager shaderManager) 
            : base(appDataDirectory, stoneProvider, collectionStore, hotkeyStore, biosManager, saveManager)
        {
            this.ProcessHandler = processHandler;
            this.CorePath = Path.Combine(this.PluginDataPath, this.PluginProperties.Get("retroarch_core"));
            this.ShaderManager = shaderManager;
        }

        public override IHotkeyTemplate GetHotkeyTemplate() => this.HotkeyTemplateStore.GetTemplate<RetroarchHotkeyTemplate>();

        [Obsolete("DEBUG ONLY")]
        public override IEmulatorInstance Instantiate(IGameRecord gameRecord, ICoreService service)
        {
            var devices = service.Get<IPluginManager>()?.Get<IInputEnumerator>()["InputEnumerator-Keyboard"].GetConnectedDevices().First();
            var contrl = service.Get<IStoneProvider>().Controllers["NES_CONTROLLER"];
            var profile = MappedControllerElementCollection.GetDefaultMappings(devices.DeviceLayout, contrl);
            var port = new EmulatedPort(0, contrl, devices, profile);
            return this.Instantiate(gameRecord, gameRecord.Files.First(f => this.Mimetypes.Contains(f.MimeType, StringComparer.InvariantCultureIgnoreCase)), 0, new List<IEmulatedPort> {port});
        }
    }
}
