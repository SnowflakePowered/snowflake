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
        public decimal FastforwardRatio { get; set; } = 0.000000M;

        [ConfigurationOption("slowmotion_ratio", DisplayName = "Slowmotion Ratio")]
        public decimal SlowmotionRatio { get; set; } = 3.000000M;

        public FramethrottleConfiguration() : base("framethottle", "Framethrottle", "retroarch.cfg")
        {
        }
    }
}
