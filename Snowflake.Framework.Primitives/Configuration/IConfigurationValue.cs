using System;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents a unique configuration value.
    /// </summary>
    public interface IConfigurationValue
    {
        /// <summary>
        /// The value of the vlaue
        /// </summary>
        object Value { get; set; }
        /// <summary>
        /// The GUID record of the value.
        /// </summary>
        Guid Guid { get; }
    }
}