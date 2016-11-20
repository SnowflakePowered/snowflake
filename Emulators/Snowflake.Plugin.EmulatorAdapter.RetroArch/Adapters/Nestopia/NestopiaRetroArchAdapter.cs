using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Extensibility;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia.Configuration;
using Snowflake.Plugin.Emulators.RetroArch.Executable;
using Snowflake.Plugin.Emulators.RetroArch.Shaders;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Service;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia
{
    [Plugin("RetroArchNestopia")]
    public class NestopiaRetroArchAdapter : RetroArchCommonAdapter
    {
        public NestopiaRetroArchAdapter(string appDataDirectory,
            RetroArchProcessHandler processHandler, IStoneProvider stoneProvider,
            IConfigurationCollectionStore collectionStore,
            IBiosManager biosManager, ISaveManager saveManager,
            ShaderManager shaderManager)
            : base(
                appDataDirectory, processHandler, stoneProvider, collectionStore, biosManager, saveManager, shaderManager)
        {
           
        }

        public override IEmulatorInstance Instantiate(IGameRecord gameRecord, IFileRecord file, int saveSlot,
            IList<IEmulatedPort> ports)
        {

            var configurations = this.GetConfiguration(gameRecord);
            var platform = this.StoneProvider.Platforms[gameRecord.PlatformId];

            return new RetroArchInstance(gameRecord, file, this, this.CorePath, this.ProcessHandler, saveSlot, platform, ports)
            {
                ShaderManager =  this.ShaderManager
            };
        }

        public override IConfigurationCollection GetConfiguration(IGameRecord gameRecord, string profileName = "default")
        {
            return this.CollectionStore.Get<NestopiaConfiguration>(gameRecord.Guid, this.PluginName, profileName);
        }

        public override IConfigurationCollection GetConfiguration()
        {
            return new ConfigurationCollection<NestopiaConfiguration>();
        }


    }
}
