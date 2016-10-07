using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

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
        /// A description of what this section does
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The GUID of this section, a name-based GUID calculated with the namespace
        /// 1768a7f5-a179-4c03-b4cb-6527ff008559 on the format {ConfigurationFileName}::{SectionName},
        /// with an empty string for configuration filename if there is no configuration filename set.
        /// </summary>
        Guid SectionGuid { get; }
        /// <summary>
        /// The options of this configuration section, keyed on the key name.
        /// </summary>
        IReadOnlyDictionary<string, IConfigurationOption> Options { get; }
        
    }
}
