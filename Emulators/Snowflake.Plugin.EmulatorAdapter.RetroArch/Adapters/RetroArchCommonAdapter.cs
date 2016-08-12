using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input;
using Snowflake.Extensibility;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters
{
    [Plugin("RetroArchCommon")]
    public class RetroArchCommonAdapter : Emulator.EmulatorAdapter
    {
        public RetroArchCommonAdapter(string appDataDirectory) : base(appDataDirectory)
        {
        }
    }
}
