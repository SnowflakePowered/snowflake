using Snowflake.Configuration;

namespace Snowflake.Plugin.Emulators.RetroArch.Configuration
{
    [ConfigurationSection("config", "Configuration Options")]
    public partial interface ConfigConfiguration
    {
        [ConfigurationOption("config_save_on_exit", false, DisplayName = "Save Config on exit", Private = true)]
        bool ConfigSaveOnExit { get; set; }

        [ConfigurationOption("auto_overrides_enable", false, DisplayName = "Automatically load config overrides",
            Private = true)]
        bool AutoOverridesEnable { get; set; }
    }
}
