using System;
using System.Collections.Generic;
using System.IO;
using Snowflake.Adapters.Higan;
using Snowflake.Configuration;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Process;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Plugin.Emulators.RetroArch
{
    public class RetroArchComposable : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IContentDirectoryProvider))]
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IConfigurationCollectionStore))]
        [ImportService(typeof(IEmulatorTaskRootDirectoryProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var stone = serviceContainer.Get<IStoneProvider>();
            var emucdp = serviceContainer.Get<IEmulatorTaskRootDirectoryProvider>();
            var ccs = serviceContainer.Get<IConfigurationCollectionStore>();

            var pm = serviceContainer.Get<IPluginManager>();
            var appdata = serviceContainer.Get<IContentDirectoryProvider>();
            var log = serviceContainer.Get<ILogProvider>().GetLogger("RetroArch");
            string appDataDirectory = appdata.ApplicationData.FullName;
            // var shaderManager = new ShaderManager(processHandler.Provision.ContentDirectory.CreateSubdirectory("shaders").FullName);
            var higanProvision = pm.GetProvision<HiganSnesAdapter>(composableModule);
            pm.Register<IEmulator>(new HiganSnesAdapter(higanProvision, stone, emucdp, ccs));
        }
    }
}
