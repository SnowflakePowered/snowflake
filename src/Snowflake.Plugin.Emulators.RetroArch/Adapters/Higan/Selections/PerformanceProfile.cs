using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

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
