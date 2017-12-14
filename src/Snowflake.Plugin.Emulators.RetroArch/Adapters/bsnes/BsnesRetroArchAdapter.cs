using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.EmulatorOld;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.bsnes;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.bsnes.Configuration;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia.Configuration;
using Snowflake.Plugin.Emulators.RetroArch.Executable;
using Snowflake.Plugin.Emulators.RetroArch.Shaders;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Services;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.Bsnes
{
    [Plugin("RetroArchBsnes")]
    public class BsnesRetroArchAdapter : RetroArchCommonAdapter
    {
        public BsnesRetroArchAdapter(IPluginProvision provision,
            RetroArchProcessHandler processHandler, IStoneProvider stoneProvider,
            IConfigurationCollectionStore collectionStore,
            IBiosManager biosManager,
            ISaveManager saveManager,
            ShaderManager shaderManager)
            : base(provision, processHandler, stoneProvider, collectionStore, biosManager, saveManager, shaderManager)
        {
        }

        /// <inheritdoc/>
        public override IEmulatorInstance Instantiate(IGameRecord gameRecord, IFileRecord file, int saveSlot,
            IList<IEmulatedPort> ports)
        {
            var platform = this.StoneProvider.Platforms[gameRecord.PlatformID];

            return new BsnesInstance(gameRecord, file, this,
                this.CorePath.FullName, this.ProcessHandler,
                saveSlot, platform, ports)
            {
                ShaderManager = this.ShaderManager,
            };
        }

        /// <inheritdoc/>
        public override IConfigurationCollection GetConfiguration(IGameRecord gameRecord, string profileName = "default")
        {
            return this.CollectionStore.Get<BsnesConfiguration>(gameRecord.Guid, this.Name, profileName);
        }

        /// <inheritdoc/>
        public override IConfigurationCollection GetConfiguration()
        {
            return new ConfigurationCollection<BsnesConfiguration>();
        }
    }
}
