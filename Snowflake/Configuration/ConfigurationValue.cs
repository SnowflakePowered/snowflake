using System;

namespace Snowflake.Configuration
{
    public class ConfigurationValue : IConfigurationValue
    {
        public object Value { get; set; }
        public IConfigurationOption Option { get; }
        public Guid Guid { get; }

        public ConfigurationValue(IConfigurationOption option, object value) : this(option, value, Guid.NewGuid())
        {
        }

        public ConfigurationValue(IConfigurationOption option, object value, Guid guid)
        {
            this.Guid = guid;
            this.Option = option;
            this.Value = value;
        }
    }
}
