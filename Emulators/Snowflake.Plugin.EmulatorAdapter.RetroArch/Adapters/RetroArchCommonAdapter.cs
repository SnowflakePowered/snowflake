using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
using Snowflake.Plugin.Emulators.RetroArch.Executable;
using Snowflake.Plugin.Emulators.RetroArch.Shaders;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters
{
    public abstract class RetroArchCommonAdapter : EmulatorAdapter
    {
        protected RetroArchProcessHandler ProcessHandler { get; }
        protected ShaderManager ShaderManager { get; }
        public string CorePath { get; }

        protected RetroArchCommonAdapter(string appDataDirectory, 
            RetroArchProcessHandler processHandler,
            IStoneProvider stoneProvider,
            IConfigurationCollectionStore collectionStore,
            IBiosManager biosManager,
            ISaveManager saveManager, 
            ShaderManager shaderManager) 
            : base(appDataDirectory, stoneProvider, collectionStore,biosManager, saveManager)
        {
            this.ProcessHandler = processHandler;
            this.CorePath = Path.Combine(this.PluginDataPath, this.PluginProperties.Get("retroarch_core"));
            this.ShaderManager = shaderManager;
        }

    }
}
