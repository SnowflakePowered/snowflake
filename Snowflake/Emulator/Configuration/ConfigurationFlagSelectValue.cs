namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationFlagSelectValue : IConfigurationFlagSelectValue
    {
        public string Value { get; }
        public string Description { get; set; }
        public ConfigurationFlagSelectValue(string value, string description)
        {
            this.Value = value;
            this.Description = description;
        }
    }
}
