using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents one section in an emulator configuration. 
    /// Inherit from this class and add your own properties, which will be serialized to a configuration file.
    /// </summary>
    //todo extend this doc
    public interface IConfigurationSection
    {
        /// <summary>
        /// The name of the section as it appears in the emulator configuration file
        /// </summary>
        string SectionName { get; }
        /// <summary>
        /// The display name of the section for human-readable purposes
        /// </summary>
        string DisplayName { get; }
        /// <summary>
        /// The file this configuration section appears in
        /// </summary>
        string ConfigurationFileName { get; }
    }
}
