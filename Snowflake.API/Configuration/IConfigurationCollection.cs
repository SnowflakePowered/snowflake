using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    /// <summary>
    /// A configuration collection represents a single file of configuration.
    /// One file can have one single serializer for every section, and one 
    /// filename for the configuration collection.
    /// </summary>
    public interface IConfigurationCollection
    {
        /// <summary>
        /// The configuration serializer for this collection
        /// </summary>
        IConfigurationSerializer Serializer { get; }

        /// <summary>
        /// The filename for this collection.
        /// </summary>
        string FileName { get; }
    }
}
