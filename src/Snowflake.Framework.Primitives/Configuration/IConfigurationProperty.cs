using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents a property in a configuration section
    /// </summary>
    public interface IConfigurationProperty
    {
        /// <summary>
        /// Gets the value of the property
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Gets the metadata associated with this property
        /// </summary>
        ConfigurationOptionAttribute Metadata { get; }
    }
}
