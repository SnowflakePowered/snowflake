using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.Emulators.RetroArch.Configuration
{
    [ConfigurationSection("config", "Configuration Options")]
    public interface ConfigConfiguration : IConfigurationSection<ConfigConfiguration>
    {
        [ConfigurationOption("config_save_on_exit", false, DisplayName = "Save Config on exit", Private = true)]
        bool ConfigSaveOnExit { get; set; }

        [ConfigurationOption("auto_overrides_enable", false, DisplayName = "Automatically load config overrides",
            Private = true)]
        bool AutoOverridesEnable { get; set; }
    }
}
