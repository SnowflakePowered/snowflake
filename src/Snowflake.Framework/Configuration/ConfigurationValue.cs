using System;

namespace Snowflake.Configuration
{
    internal class ConfigurationValue : IConfigurationValue
    {
        /// <inheritdoc/>
        public object Value { get; set; }

        /// <inheritdoc/>
        public Guid Guid { get; }

        public ConfigurationOptionType Type { get; }

        internal ConfigurationValue(object value, ConfigurationOptionType type)
            : this(value, Guid.NewGuid(), type)
        {
        }

        internal ConfigurationValue(object value, Guid guid, ConfigurationOptionType type)
        {
            this.Guid = guid;
            this.Value = value;
            this.Type = type;
        }
    }
}
