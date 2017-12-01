using System;
using System.Collections.Generic;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Describes a configuration section
    /// </summary>
    public interface IConfigurationSectionDescriptor 
    {
        /// <summary>
        /// The description of the section
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The human readable name of the section
        /// </summary>
        string DisplayName { get; }
        /// <summary>
        /// The name of the section as it appears in configuration.
        /// </summary>
        string SectionName { get; }
        /// <summary>
        /// The list of configuration options in the section
        /// ordered as they were declared as properties.
        /// The implementation is responsible for ensuring this is immutable
        /// and correctly ordered.
        /// </summary>
        IEnumerable<IConfigurationOptionDescriptor> Options { get; }
        /// <summary>
        /// Gets the configuration option with the specified property name
        /// </summary>
        /// <param name="optionKey">The property name of the option as declared</param>
        /// <returns>The configuration option</returns>
        IConfigurationOptionDescriptor this[string optionKey] { get; }
    }
}