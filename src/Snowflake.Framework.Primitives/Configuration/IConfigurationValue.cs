using System;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents a unique configuration value.
    /// </summary>
    public interface IConfigurationValue
    {
        /// <summary>
        /// Gets or sets the value of the ConfigurationValue
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// Gets the GUID record of the value.
        /// </summary>
        Guid Guid { get; }
    }
}
