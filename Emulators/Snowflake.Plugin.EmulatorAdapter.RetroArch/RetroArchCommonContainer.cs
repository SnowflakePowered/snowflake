using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Extensibility;
using Snowflake.Plugin.Emulators.RetroArch.Adapters;

using Snowflake.Plugin.Emulators.RetroArch.Executable;
using Snowflake.Plugin.Emulators.RetroArch.Shaders;
using Snowflake.Service;
using Snowflake.Service.Manager;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Bsnes;

namespace Snowflake.Plugin.Emulators.RetroArch
{
    public class RetroArchCommonContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            var pm = coreInstance.Get<IPluginManager>();
            var processHandler = new RetroArchProcessHandler(coreInstance.AppDataDirectory);
            var shaderManager = new ShaderManager(Path.Combine(processHandler.PluginDataPath, "shaders"));
            pm.Register(processHandler);

            pm.Register(new NestopiaRetroArchAdapter(coreInstance.AppDataDirectory,
                processHandler,
                coreInstance.Get<IStoneProvider>(),
                coreInstance.Get<IConfigurationCollectionStore>(), 
                new BiosManager(coreInstance.AppDataDirectory),
                new SaveManager(coreInstance.AppDataDirectory), shaderManager));

            pm.Register(new BsnesRetroArchAdapter(coreInstance.AppDataDirectory,
               processHandler,
               coreInstance.Get<IStoneProvider>(),
               coreInstance.Get<IConfigurationCollectionStore>(),
               new BiosManager(coreInstance.AppDataDirectory),
               new SaveManager(coreInstance.AppDataDirectory), shaderManager));
        }
    }
}
