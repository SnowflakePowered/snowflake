using System;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents a store that can save and retrieve an aribtrary configuration
    /// collection representing a single emulator configuration file, associated with a game record
    /// </summary>
    /// <remarks>
    /// To "delete" a configuration, just overwrite the existing values with a default instance
    /// </remarks>
    public interface IConfigurationCollectionStore
    {
        /// <summary>
        /// Retrieves the configuration collection associated with this game record.
        /// This method is guaranteed to return a usable instance of the configuration collection.
        /// If a prior configuration has not been set, it should return a default instance with all
        /// properties initialized.
        ///
        /// <para>
        /// This function should return the same configuration values with identical GUIDs for the same
        /// database instance. This may imply that the function is impure; the default implementation
        /// will save every retrieved database using <see cref="Set{T}(IConfigurationCollection{T}, Guid, string, string)"/>.
        /// internally. In other words, retrieval of a configuration may imply the persistence of the same configuration.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The type of configuration collection</typeparam>
        /// <param name="gameRecord">The guid of the game record</param>
        /// <param name="emulator">The name of the emulator this collection belongs to</param>
        /// <param name="profile">The profile this collection belongs to</param>
        /// <returns>A valid configuration collection of type T</returns>
        IConfigurationCollection<T> Get<T>(Guid gameRecord, string emulator, string profile)
            where T : class, IConfigurationCollection<T>;

        /// <summary>
        /// Saves and persists a configuration collection to the store.
        /// </summary>
        /// <param name="configuration">The configuration to save to the store</param>
        /// <param name="gameRecord">The guid of the game record associated with this configuration collection</param>
        /// <param name="emulator">The name of the emulator this collection belongs to</param>
        /// <param name="profile">The profile this collection belongs to</param>
        void Set<T>(IConfigurationCollection<T> configuration, Guid gameRecord, string emulator, string profile)
            where T : class, IConfigurationCollection<T>;

        /// <summary>
        /// Updates a single <em>existing</em> configuration value, this will error if the GUID is not found in
        /// the database.
        /// </summary>
        /// <param name="value">The configuration value with valid GUID and updated data</param>
        void Set(IConfigurationValue value);
    }
}