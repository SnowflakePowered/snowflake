using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Snowflake.Plugin
{
    /// <summary>
    /// Represents a configuration file a plugin can access.
    /// The file must be serializable to IDictionary<string, dynamic>
    /// The plugin must initialize this itself in it's constructor. 
    /// </summary>
    public interface IPluginConfiguration
    {
        /// <summary>
        /// The name of the configuration file, commonly file.json or file.yml
        /// </summary>
        string ConfigurationFileName { get; }
        /// <summary>
        /// The configuration keys of the file
        /// </summary>
        IDictionary<string, dynamic> Configuration { get; }
        /// <summary>
        /// Load the configuration from file to memory
        /// </summary>
        void LoadConfiguration();
        /// <summary>
        /// Save the configuration from memory to disk
        /// </summary>
        void SaveConfiguration();
    }
}
