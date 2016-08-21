using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Process
{
    [Plugin("exe-retroarch-win64")]
    public class RetroArchProcessHandler : Extensibility.Plugin
    {
        public RetroArchProcessHandler(string appDataDirectory, IPluginProperties pluginProperties) : base(appDataDirectory, pluginProperties)
        {
        }

        public RetroArchProcessHandler(string appDataDirectory) : base(appDataDirectory)
        {
        }

        internal RetroArchProcessInfo GetProcessInfo(string romFilePath) => new RetroArchProcessInfo(this.PluginDataPath, romFilePath);
    }
}
