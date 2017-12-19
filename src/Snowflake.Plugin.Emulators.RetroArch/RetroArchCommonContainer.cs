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
using Snowflake.Loader;
using Snowflake.Plugin.Emulators.RetroArch.Adapters;
using Snowflake.Plugin.Emulators.RetroArch.Shaders;
using Snowflake.Services;

namespace Snowflake.Plugin.Emulators.RetroArch
{
    public class RetroArchCommonContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IContentDirectoryProvider))]
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var pm = serviceContainer.Get<IPluginManager>();
            var appdata = serviceContainer.Get<IContentDirectoryProvider>();
            var log = serviceContainer.Get<ILogProvider>().GetLogger("RetroArch");
            string appDataDirectory = appdata.ApplicationData.FullName;

            var processHandlerProvision = pm.GetProvision<RetroArchProcessHandler>(composableModule);
            // var shaderManager = new ShaderManager(processHandler.Provision.ContentDirectory.CreateSubdirectory("shaders").FullName);
        }
    }
}
