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
using Snowflake.Services;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Bsnes;
using Snowflake.Loader;

namespace Snowflake.Plugin.Emulators.RetroArch
{
    public class RetroArchCommonContainer : IComposer
    {
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IContentDirectoryProvider))]
        [ImportService(typeof(IStoneProvider))]
        public void Compose(IServiceContainer coreInstance)
        {
            var pm = coreInstance.Get<IPluginManager>();
            var appdata = coreInstance.Get<IContentDirectoryProvider>();
            string appDataDirectory = appdata.ApplicationData.FullName;
            var processHandler = new RetroArchProcessHandler(appDataDirectory);
            var shaderManager = new ShaderManager(Path.Combine(processHandler.PluginDataPath, "shaders"));
            pm.Register(processHandler);

            pm.Register(new NestopiaRetroArchAdapter(appDataDirectory,
                processHandler,
                coreInstance.Get<IStoneProvider>(),
                coreInstance.Get<IConfigurationCollectionStore>(), 
                new BiosManager(appDataDirectory),
                new SaveManager(appDataDirectory), shaderManager));

            pm.Register(new BsnesRetroArchAdapter(appDataDirectory,
               processHandler,
               coreInstance.Get<IStoneProvider>(),
               coreInstance.Get<IConfigurationCollectionStore>(),
               new BiosManager(appDataDirectory),
               new SaveManager(appDataDirectory), shaderManager));
        }
    }
}
