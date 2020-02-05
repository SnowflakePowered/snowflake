using System;
using System.Collections.Generic;
using System.IO;
using Snowflake.Adapters.Higan;
using Snowflake.Configuration;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Process;
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
        [ImportService(typeof(IEmulatorExecutableProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var stone = serviceContainer.Get<IStoneProvider>();
            var exp = serviceContainer.Get<IEmulatorExecutableProvider>();
            var pm = serviceContainer.Get<IPluginManager>();
            var log = serviceContainer.Get<ILogProvider>().GetLogger("RetroArch");
            var retroArchExecutable = exp.GetEmulator("retroarch");

            var provision = pm.GetProvision<RetroArchBsnesOrchestrator>(composableModule);
            pm.Register<IEmulatorOrchestrator>(new RetroArchBsnesOrchestrator(retroArchExecutable, provision));
            // var shaderManager = new ShaderManager(processHandler.Provision.ContentDirectory.CreateSubdirectory("shaders").FullName);
            //var higanProvision = pm.GetProvision<HiganSnesAdapter>(composableModule);
            //pm.Register<IEmulator>(new HiganSnesAdapter(higanProvision, stone, emucdp, exe));
        }
    }
}
