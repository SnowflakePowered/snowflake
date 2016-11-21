﻿using System.Collections.Generic;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    /// <summary>
    /// A configuration collection represents a single file of configuration.
    /// One file can have one single serializer for every section, and multiple 
    /// filename for the configuration collection.
    /// A configuration collection must be marked with one or more <see cref="ConfigurationFileAttribute"/>,
    /// and all seriazable properties marked with a <see cref="SerializableSectionAttribute"/> that matches one or more configuration files.
    ///
    /// The enumeration is guaranteed to enumerate in the order in which the section properties were described.
    /// </summary>
    /// <typeparam name="T">The type of the configuration collection</typeparam>
    public interface IConfigurationCollection<out T> : IConfigurationCollection where T : class, IConfigurationCollection<T>
    {
        /// <summary>
        /// The configuration instance which holds the configuration sections.
        /// </summary>
        T Configuration { get; }
    }
    /// <summary>
    /// A configuration collection represents a single file of configuration.
    /// One file can have one single serializer for every section, and multiple 
    /// filename for the configuration collection.
    /// A configuration collection must be marked with one or more <see cref="ConfigurationFileAttribute"/>,
    /// and all seriazable properties marked with a <see cref="SerializableSectionAttribute"/> that matches one or more configuration files.
    ///
    /// The enumeration is guaranteed to enumerate in the order in which the section properties were described.
    /// </summary>
    public interface IConfigurationCollection : IEnumerable<KeyValuePair<string, IConfigurationSection>>
    {
        /// <summary>
        /// The descriptor that describes this configuration collection.
        /// </summary>
        IConfigurationCollectionDescriptor Descriptor { get; }
        /// <summary>
        /// Gets a configuration section by it's property name
        /// </summary>
        /// <param name="sectionName">The property name of the section</param>
        /// <returns>The untyped configuration section with the given property name.</returns>
        IConfigurationSection this[string sectionName] { get; }

    }
}
