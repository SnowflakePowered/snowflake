using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch
{
    public class RetroArchCommonContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            coreInstance.Get<IPluginManager>().Register(new RetroArchCommonAdapter(coreInstance.AppDataDirectory));
        }
    }
}
