using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters.bsnes.Selections;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters.bsnes.Configuration
{
    public class BsnesConfiguration : ConfigurationSection
    {
        [ConfigurationOption("flag#performanceprofile", DisplayName = "Performance Profile", Flag = true)]
        public PerformanceProfile PerformanceProfile { get; set; } = PerformanceProfile.Performance;

    
        public BsnesConfiguration() : base("bsnes", "bsnes Settings", "bsnes Core Options")
        {
        }
    }
}
