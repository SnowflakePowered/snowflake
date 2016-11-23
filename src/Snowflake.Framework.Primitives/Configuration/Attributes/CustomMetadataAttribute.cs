using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Attributes
{
    
    /// <summary>
    /// Represents a custom metadata on a selection option or a configuration property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class CustomMetadataAttribute : Attribute
    {
        /// <summary>
        /// The metadata key
        /// </summary>
        public string Key { get; }
        /// <summary>
        /// The value of the metadata
        /// </summary>
        public object Value { get; }
        public CustomMetadataAttribute(string metadataKey, object value)
        {
            this.Key = metadataKey;
            this.Value = value;
        }
    }
}
