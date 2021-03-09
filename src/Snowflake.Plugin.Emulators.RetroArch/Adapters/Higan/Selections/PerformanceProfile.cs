using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.Higan.Selections
{
    public enum PerformanceProfile
    {
        [SelectionOption("#flag", DisplayName = "Performance")]
        Performance,

        [SelectionOption("#flag", DisplayName = "Performance")]
        Balanced,

        [SelectionOption("$flag", DisplayName = "Performance")]
        Accuracy,
    }
}
