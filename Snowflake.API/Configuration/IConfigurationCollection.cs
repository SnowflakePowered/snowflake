using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents a collection of configuration sections.
    /// Configuration sections can be split across multiple files
    /// 
    /// This does not include input configuration.
    /// </summary>
    public interface IConfigurationCollection : IEnumerable<KeyValuePair<IConfigurationSection, Type>>
    {
        /// <summary>
        /// Adds a configuration section to the collection
        /// </summary>
        /// <param name="configurationSection"></param>
        void Add<TConfiguration, TSerializer>(string key, TConfiguration configurationSection)
            where TConfiguration : class, IConfigurationSection, new()
            where TSerializer : class, IConfigurationSerializer, new();
        
        /// <summary>
        /// Gets the specified configuration section
        /// </summary>
        /// <typeparam name="TConfiguration">The type of the configuration section</typeparam>
        /// <returns>The type of the configuration section, null if not exists.</returns>
        TConfiguration Get<TConfiguration>(string key) where TConfiguration : class, IConfigurationSection, new ();
    }
}
