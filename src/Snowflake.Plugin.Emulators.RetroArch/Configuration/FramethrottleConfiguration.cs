using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.Emulators.RetroArch.Configuration
{
    [ConfigurationSection("framethottle", "Framethrottle")]
    public partial interface FramethrottleConfiguration
    {
        [ConfigurationOption("fastforward_ratio", 0.000000, DisplayName = "Maximum Run Speed")]
        double FastforwardRatio { get; set; }

        [ConfigurationOption("slowmotion_ratio", 3.000000, DisplayName = "Slowmotion Ratio")]
        double SlowmotionRatio { get; set; }
    }
}
