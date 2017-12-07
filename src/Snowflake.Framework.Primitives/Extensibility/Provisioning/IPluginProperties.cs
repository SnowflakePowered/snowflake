using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Extensibility.Provisioning
{
    /// <summary>
    /// Represents the properties defined inside a plugin.json
    /// </summary>
    public interface IPluginProperties
    {
        /// <summary>
        /// Gets all the properties defined
        /// </summary>
        IEnumerable<string> PropertyKeys { get; }

        /// <summary>
        /// Gets a single property
        /// </summary>
        /// <param name="key">The key of the property</param>
        /// <returns>The property to get</returns>
        string Get(string key);

        /// <summary>
        /// Gets a list of properties
        /// </summary>
        /// <param name="key">The key of the property list</param>
        /// <returns>The list of properties</returns>
        IEnumerable<string> GetEnumerable(string key);

        /// <summary>
        /// Gets a dictionary of properties
        /// </summary>
        /// <param name="key">The key of the property dictionary</param>
        /// <returns>The dictionary of properties</returns>
        IDictionary<string, string> GetDictionary(string key);
    }
}
