using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Extensibility;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters.Nestopia;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Executable;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch
{
    public class RetroArchCommonContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            var pm = coreInstance.Get<IPluginManager>();
            var processHandler = new RetroArchProcessHandler(coreInstance.AppDataDirectory);
            pm.Register(processHandler);

            pm.Register(new NestopiaRetroArchAdapter(coreInstance.AppDataDirectory,
                processHandler,
                coreInstance.Get<IStoneProvider>(),
                coreInstance.Get<IConfigurationCollectionStore>(), new BiosManager(coreInstance.AppDataDirectory),
                new SaveManager(coreInstance.AppDataDirectory)));
        }
    }
}
