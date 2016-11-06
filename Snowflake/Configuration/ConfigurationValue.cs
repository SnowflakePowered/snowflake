using System;

namespace Snowflake.Configuration
{
    public class ConfigurationValue : IConfigurationValue
    {
        public object Value { get; set; }
        public Guid Guid { get; }
        
        internal ConfigurationValue(object value) : this(value, Guid.NewGuid())
        {
        }

        internal ConfigurationValue(object value, Guid guid)
        {
            this.Guid = guid;
            this.Value = value;
        }
    }
}
