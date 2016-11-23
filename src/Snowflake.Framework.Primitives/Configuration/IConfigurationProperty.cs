using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents a property in a configuration section
    /// </summary>
    public interface IConfigurationProperty
    {
        /// <summary>
        /// The value of the property
        /// </summary>
        object Value { get; }

        /// <summary>
        /// The metadata associated with this property
        /// </summary>
        ConfigurationOptionAttribute Metadata { get; }
    }
}
