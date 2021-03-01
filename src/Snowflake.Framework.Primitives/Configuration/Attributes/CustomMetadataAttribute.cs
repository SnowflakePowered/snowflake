using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents a custom metadata on a selection option or a configuration property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class CustomMetadataAttribute : Attribute
    {
        /// <summary>
        /// Gets the metadata key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the value of the metadata
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Mark a selection option or configuration property as custom metadata.
        /// Custom metadata is never serialized to file, and is only readable
        /// by the emulator bridge plugin.
        /// </summary>
        /// <param name="metadataKey">The key of the metadata.</param>
        /// <param name="value">The metadata value.</param>
        public CustomMetadataAttribute(string metadataKey, object value)
        {
            this.Key = metadataKey;
            this.Value = value;
        }
    }
}
