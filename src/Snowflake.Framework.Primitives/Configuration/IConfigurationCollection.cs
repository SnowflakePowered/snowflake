using Snowflake.Configuration.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Snowflake.Configuration
{
    /// <summary>
    /// A configuration collection represents a single file of configuration.
    /// One file can have one single serializer for every section, and multiple
    /// filename for the configuration collection.
    ///
    /// The enumeration is guaranteed to enumerate in the order in which the section properties were described.
    /// </summary>
    /// <typeparam name="T">The type of the configuration collection</typeparam>
    [GenericTypeAcceptsConfigurationCollection(0)]
    public interface IConfigurationCollection<T> : IConfigurationCollection
        where T : class
    {
        /// <summary>
        /// Gets the configuration instance which holds the configuration sections.
        /// </summary>
        T Configuration { get; }

        /// <summary>
        /// Gets a configuration section by its expression.
        /// </summary>
        /// <param name="expression">The expression that locates the property.</param>
        /// <returns>The typed configuration section with the given property name.</returns>
        [GenericTypeAcceptsConfigurationSection(0)]
        IConfigurationSection<TSection> GetSection<TSection>(Expression<Func<T, TSection>> expression)
            where TSection: class;
    }

    /// <summary>
    /// A configuration collection represents a single file of configuration.
    /// One file can have one single serializer for every section, and multiple
    /// filename for the configuration collection.
    ///
    /// The enumeration is guaranteed to enumerate in the order in which the section properties were described.
    /// </summary>
    public interface IConfigurationCollection : IEnumerable<KeyValuePair<string, IConfigurationSection>>
    {
        /// <summary>
        /// Gets the descriptor that describes this configuration collection.
        /// </summary>
        IConfigurationCollectionDescriptor Descriptor { get; }

        /// <summary>
        /// Gets a configuration section by it's property name
        /// </summary>
        /// <param name="sectionKey">The property name of the section</param>
        /// <returns>The untyped configuration section with the given property name.</returns>
        IConfigurationSection? GetSection(string sectionKey);

        /// <summary>
        /// Gets the values backing this configuration collection.
        /// </summary>
        IConfigurationValueCollection ValueCollection { get; }

        /// <summary>
        /// The unique GUID of the collection is defined by its value collection GUID.
        /// </summary>
        Guid CollectionGuid => ValueCollection.Guid;
    }
}
