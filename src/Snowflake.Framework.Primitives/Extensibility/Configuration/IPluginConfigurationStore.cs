using System;
using System.Collections.Generic;
using Snowflake.Configuration;

namespace Snowflake.Extensibility.Configuration
{
    /// <summary>
    /// Represents a store that can save and retrieve an aribtrary configuration
    /// collection representing a single emulator configuration file, associated with a game record
    /// </summary>
    /// <remarks>
    /// To "delete" a configuration, just overwrite the existing values with a default instance
    /// </remarks>
    public interface IPluginConfigurationStore
    {
        /// <summary>
        /// Retrieves the configuration collection associated with this game record.
        /// This method is guaranteed to return a usable instance of the configuration collection.
        /// If a prior configuration has not been set, it should return a default instance with all
        /// properties initialized.
        /// </summary>
        /// <typeparam name="T">The type of configuration collection</typeparam>
        /// <returns>The configuration collection associated with this game record. </returns>
        IConfigurationSection<T> Get<T>()
            where T : class, IConfigurationSection<T>;

        /// <summary>
        /// Saves and persists a configuration collection to the store.
        /// </summary>
        /// <typeparam name="T">The type of configuration collection</typeparam>
        /// <param name="configuration">The configuration to save to the store</param>
        void Set<T>(IConfigurationSection<T> configuration)
            where T : class, IConfigurationSection<T>;

        /// <summary>
        /// Updates a single <em>existing</em> configuration value, this will error if the GUID is not found in
        /// the database.
        /// </summary>
        /// <param name="value">The configuration value with valid GUID and updated data</param>
        void Set(IConfigurationValue value);

        /// <summary>
        /// Updates multiple <em>existing</em> configuration values, this will error if the GUID is not found in
        /// the database.
        /// </summary>
        /// <param name="values">The configuration value with valid GUID and updated data</param>
        void Set(IEnumerable<IConfigurationValue> values);
    }
}