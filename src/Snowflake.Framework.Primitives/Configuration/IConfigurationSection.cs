using System.Collections.Generic;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents a serializable section in a configuration
    /// </summary>
    /// <typeparam name="T">The type of configuration</typeparam>
    public interface IConfigurationSection<out T> : IConfigurationSection
        where T : class, IConfigurationSection<T>
    {
        /// <summary>
        /// Gets the typed section instance which holds the configuration values
        /// </summary>
        T Configuration { get; }
    }

    /// <summary>
    /// Represents a serializable section in a configuration
    /// </summary>
    public interface IConfigurationSection : IEnumerable<KeyValuePair<IConfigurationOptionDescriptor, IConfigurationValue>>
    {
        /// <summary>
        /// Gets the descriptor that describes the configuration section.
        /// </summary>
        IConfigurationSectionDescriptor Descriptor { get; }

        /// <summary>
        /// Gets the read only mapping of property names to configuration values
        /// Enumerating over the values key is not guaranteed to be in the same
        /// order as the order the properties were defined.
        ///
        /// The implementation is responsible for ensuring this mapping synced with the
        /// values stored in the object and ensuring this mapping is immutable.
        /// </summary>
        IReadOnlyDictionary<string, IConfigurationValue> Values { get; }

        /// <summary>
        /// Gets the values backing this configuration section,
        /// as well as those in the parent configuration collection.
        /// 
        /// It is well recommended to use <see cref="IConfigurationSection.Values"/> instead,
        /// however direct access to the value collection may be useful in some cases.
        /// </summary>
        IConfigurationValueCollection ValueCollection { get; }

        /// <summary>
        /// Gets or sets the option value with the specified property name in the configuration section
        /// in an untyped and unsafe manner.
        /// </summary>
        /// <remarks>Only use this if you know what you are doing. The safe manner to access configuration values is
        /// through the <see cref="IConfigurationSection{T}.Configuration"/> property.</remarks>
        /// <param name="key">The property name of the configuration option</param>
        /// <returns>The untyped value of the configuration value</returns>
        object? this[string key] { get; set; }
    }
}
