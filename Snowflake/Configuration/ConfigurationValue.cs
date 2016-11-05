using System;

namespace Snowflake.Configuration
{
    public class ConfigurationValue : IConfigurationValue
    {
        public object Value { get; set; }
        public Guid Guid { get; }
        
        public ConfigurationValue(object value) : this(value, Guid.NewGuid())
        {
        }

        public ConfigurationValue(object value, Guid guid)
        {
            this.Guid = guid;
            this.Value = value;
        }
    }
}
