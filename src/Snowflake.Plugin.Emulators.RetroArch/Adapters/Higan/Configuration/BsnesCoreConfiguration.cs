using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Higan.Selections;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.Higan.Configuration
{
    [ConfigurationSection("bsnes", "BSNES Settings")]
    public interface BsnesCoreConfiguration : IConfigurationSection<BsnesCoreConfiguration>
    {
        [ConfigurationOption("flag#performanceprofile", PerformanceProfile.Performance,
            DisplayName = "Performance Profile", Flag = true)]
        PerformanceProfile PerformanceProfile { get; set; }
    }
}
