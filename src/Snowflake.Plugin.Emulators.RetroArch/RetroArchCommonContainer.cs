using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Provisioned;
using Snowflake.Plugin.Emulators.RetroArch.Adapters;
using Snowflake.Plugin.Emulators.RetroArch.Executable;
using Snowflake.Plugin.Emulators.RetroArch.Shaders;
using Snowflake.Services;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Bsnes;
using Snowflake.Loader;

namespace Snowflake.Plugin.Emulators.RetroArch
{
    public class RetroArchCommonContainer : IComposable
    {
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IContentDirectoryProvider))]
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule composableModule, IServiceContainer serviceContainer)
        {
            var pm = serviceContainer.Get<IPluginManager>();
            var appdata = serviceContainer.Get<IContentDirectoryProvider>();
            var log = serviceContainer.Get<ILogProvider>().GetLogger("RetroArch");
            string appDataDirectory = appdata.ApplicationData.FullName;

            var processHandlerProvision = pm.GetProvision<RetroArchProcessHandler>(composableModule);
            var processHandler = new RetroArchProcessHandler(processHandlerProvision); //todo register as service
            var shaderManager = new ShaderManager(processHandler.Provision.ContentDirectory.CreateSubdirectory("shaders").FullName);
            pm.Register(processHandler);

            pm.Register(new NestopiaRetroArchAdapter(pm.GetProvision<NestopiaRetroArchAdapter>(composableModule),
                processHandler,
                serviceContainer.Get<IStoneProvider>(),
                serviceContainer.Get<IConfigurationCollectionStore>(), 
                new BiosManager(appDataDirectory),
                new SaveManager(appDataDirectory), shaderManager));

            pm.Register(new BsnesRetroArchAdapter(pm.GetProvision<BsnesRetroArchAdapter>(composableModule),
               processHandler,
               serviceContainer.Get<IStoneProvider>(),
               serviceContainer.Get<IConfigurationCollectionStore>(),
               new BiosManager(appDataDirectory),
               new SaveManager(appDataDirectory), shaderManager));
        }
    }
}
