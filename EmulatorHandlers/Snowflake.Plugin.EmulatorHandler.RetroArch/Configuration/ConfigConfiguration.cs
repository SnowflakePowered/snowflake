using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.EmulatorHandler.RetroArch.Configuration
{
    public class ConfigConfiguration : ConfigurationSection
    {
        [ConfigurationOption("config_save_on_exit", DisplayName = "Save Config on exit", Private = true)]
        public bool ConfigSaveOnExit { get; set; } = false;

        [ConfigurationOption("auto_overrides_enable", DisplayName = "Automatically load config overrides", Private = true)]
        public bool AutoOverridesEnable { get; set; } = false;


        public ConfigConfiguration() : base("config", "Configuration Options", "retroarch.cfg")
        {
        }
    }
}
