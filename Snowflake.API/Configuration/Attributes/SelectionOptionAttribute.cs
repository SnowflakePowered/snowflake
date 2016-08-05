using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Attributes
{
    /// <summary>
    /// Represents a selection inside an enum that represents valid values for a configuration option
    /// </summary>
    /// <seealso cref="ConfigurationOptionAttribute"/>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SelectionOptionAttribute : Attribute
    {
        /// <summary>
        /// The value to serialize this enum value as
        /// </summary>
        public string SerializeAs { get; }

        /// <summary>
        /// The display name of this value for human readable purposes
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Represents a selection inside an enum that represents valid values for a configuration option
        /// </summary>
        /// <param name="serializeAs">The value to serialize this enum as</param>
        /// <seealso cref="ConfigurationOptionAttribute"/>
        /// <seealso cref="System.Attribute" />
        public SelectionOptionAttribute(string serializeAs)
        {
            this.SerializeAs = serializeAs;
        }
    }
}
