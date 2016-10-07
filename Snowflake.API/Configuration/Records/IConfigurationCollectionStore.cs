using System;
using Snowflake.Records;

namespace Snowflake.Configuration.Records
{
    /// <summary>
    /// Represents a store that can save and retrieve an aribtrary configuration 
    /// collection representing a single emulator configuration file, associated with a game record
    /// </summary>
    /// <remarks>
    /// Because configuration values simply represent an unassociated value, retrieving a value without
    /// an object reference is not supported. Thus, while <see cref="IConfigurationCollectionStore"/> implements
    /// <see cref="ILibrary{T}"/>, individual Get methods are not supported. 
    /// </remarks>
    public interface IConfigurationCollectionStore : ILibrary<IConfigurationValue>
    {
        /// <summary>
        /// Retrieves the configuration collection associated with this game record.
        /// This method is guaranteed to return a usable instance of the configuration collection. 
        /// If a prior configuration has not been set, it should return a default instance with all
        /// properties initialized.
        /// </summary>
        /// <typeparam name="T">The type of configuration collection</typeparam>
        /// <param name="gameRecord">The guid of the game record</param>
        /// <returns>The configuration collection associated with this game record.</returns>
        T Get<T>(Guid gameRecord) where T : IConfigurationCollection, new();

        /// <summary>
        /// Saves and persists a configuration collection to the store.
        /// </summary>
        /// <param name="collection">The collection to save</param>
        /// <param name="gameRecord">The guid of the game record associated with this configuration collection</param>
        void Set(IConfigurationCollection collection, Guid gameRecord);


    }
}