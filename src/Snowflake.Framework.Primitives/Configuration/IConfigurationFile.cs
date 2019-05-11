using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    /// <summary>
    /// A target file that configuration will be serialized to.
    /// </summary>
    [Obsolete("to be replaced by configuration target dsl")]
    public interface IConfigurationFile
    {
        /// <summary>
        /// The destination of the file.
        /// </summary>
        string Destination { get; }
        /// <summary>
        /// The key(?) of the file?
        /// </summary>
        string Key { get; }
        /// <summary>
        /// Maps booleans to strings.
        /// </summary>
        IBooleanMapping BooleanMapping { get; }
    }
}
