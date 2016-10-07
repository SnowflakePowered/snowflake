using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Hotkey;
using Snowflake.Configuration.Records;
using Snowflake.Emulator;
using Snowflake.Extensibility;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters.bsnes;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters.bsnes.Configuration;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters.Nestopia.Configuration;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Executable;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Input.Hotkeys;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Shaders;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Service;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters.Bsnes
{
    [Plugin("RetroArchBsnes")]
    public class BsnesRetroArchAdapter : RetroArchCommonAdapter
    {
        public BsnesRetroArchAdapter(string appDataDirectory,
            RetroArchProcessHandler processHandler, IStoneProvider stoneProvider,
            IConfigurationCollectionStore collectionStore,
            IHotkeyTemplateStore hotkeyStore,
            IBiosManager biosManager, 
            ISaveManager saveManager,
            ShaderManager shaderManager)
            : base(
                appDataDirectory, processHandler, stoneProvider, collectionStore, hotkeyStore, biosManager, saveManager, shaderManager)
        {
           
        }

        public override IEmulatorInstance Instantiate(IGameRecord gameRecord, IFileRecord file, int saveSlot,
            IList<IEmulatedPort> ports)
        {

            var platform = this.StoneProvider.Platforms[gameRecord.PlatformId];

            return new BsnesInstance(gameRecord, file, this, this.CorePath, this.ProcessHandler, saveSlot, platform, ports)
            {
                ShaderManager =  this.ShaderManager 
            };
        }

        public override IDictionary<string, IConfigurationCollection> GetConfigurations(IGameRecord gameRecord)
        {
            var retroarchConfig = this.CollectionStore.Get<RetroArchConfiguration>(gameRecord.Guid);
            var bsnesConfig = this.CollectionStore.Get<BsnesConfigurationCollection>(gameRecord.Guid);
            return new Dictionary<string, IConfigurationCollection>
            {
                {retroarchConfig.FileName, retroarchConfig},
                {"bsnes", bsnesConfig } //convention requires core options to have this file name.
            };
       }

        public override IDictionary<string, IConfigurationCollection> GetDefaultConfigurations()
        {
            var retroarchConfig = ConfigurationCollection.MakeDefault<RetroArchConfiguration>();
            var bsnesConfig = ConfigurationCollection.MakeDefault<BsnesConfigurationCollection>();
            return new Dictionary<string, IConfigurationCollection>
            {
                {retroarchConfig.FileName, retroarchConfig},
                {"bsnes", bsnesConfig } //convention requires core options to have this file name.
            };
        }
    }
}
