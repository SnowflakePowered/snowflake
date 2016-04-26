using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.EmulatorHandler.RetroArch.Configuration
{
    public class FramethrottleConfiguration : ConfigurationSection
    {
        [ConfigurationOption("fastforward_ratio", DisplayName = "Maximum Run Speed")]
        public double FastforwardRatio { get; set; } = 0.000000;

        [ConfigurationOption("slowmotion_ratio", DisplayName = "Slowmotion Ratio")]
        public double SlowmotionRatio { get; set; } = 3.000000;

        public FramethrottleConfiguration() : base("framethottle", "Framethrottle")
        {
        }
    }
}
