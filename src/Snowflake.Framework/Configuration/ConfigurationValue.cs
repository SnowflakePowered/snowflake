using System;

namespace Snowflake.Configuration
{
    internal class ConfigurationValue : IConfigurationValue
    {
        /// <inheritdoc/>
        public object Value { get; set; }

        /// <inheritdoc/>
        public Guid Guid { get; }

        internal ConfigurationValue(object value)
            : this(value, Guid.NewGuid())
        {
        }

        internal ConfigurationValue(object value, Guid guid)
        {
            this.Guid = guid;
            this.Value = value;
        }
    }
}
