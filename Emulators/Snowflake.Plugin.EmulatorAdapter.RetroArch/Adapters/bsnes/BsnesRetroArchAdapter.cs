﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Extensibility;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.bsnes;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.bsnes.Configuration;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia.Configuration;
using Snowflake.Plugin.Emulators.RetroArch.Executable;
using Snowflake.Plugin.Emulators.RetroArch.Shaders;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Service;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.Bsnes
{
    [Plugin("RetroArchBsnes")]
    public class BsnesRetroArchAdapter : RetroArchCommonAdapter
    {
        public BsnesRetroArchAdapter(string appDataDirectory,
            RetroArchProcessHandler processHandler, IStoneProvider stoneProvider,
            IConfigurationCollectionStore collectionStore,
            IBiosManager biosManager, 
            ISaveManager saveManager,
            ShaderManager shaderManager)
            : base(appDataDirectory, processHandler, stoneProvider, collectionStore, biosManager, saveManager, shaderManager)
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


        public override IConfigurationCollection GetConfiguration(IGameRecord gameRecord, string profileName = "default")
        {
            return this.CollectionStore.Get<BsnesConfiguration>(gameRecord.Guid, this.PluginName, profileName);
        }

        public override IConfigurationCollection GetConfiguration()
        {
            return new ConfigurationCollection<BsnesConfiguration>();
        }
    }
}
