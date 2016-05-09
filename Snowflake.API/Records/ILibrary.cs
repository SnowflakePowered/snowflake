using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Records
{
    /// <summary>
    /// Represents a generic library of metadata assignable items
    /// </summary>
    /// <typeparam name="T">The type of metadata assignable item</typeparam>
    public interface ILibrary<T> where T : IRecord
    {
        /// <summary>
        /// Adds or updates a record to the library
        /// </summary>
        /// <param name="record"></param>
        void Add(T record);

        /// <summary>
        /// Removes a record from the library
        /// </summary>
        /// <param name="record"></param>
        void Remove(T record);

        /// <summary>
        /// Gets a record from the libary
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>The record</returns>
        T Get(Guid guid);

        /// <summary>
        /// Removes a record by Guid lookup
        /// </summary>
        /// <param name="guid">The guid of the record</param>
        void Remove(Guid guid);

        /// <summary>
        /// Executes a `LIKE` search on a metadata value
        /// </summary>
        /// <param name="key">The key to search for</param>
        /// <param name="likeValue">The fuzzy search value</param>
        /// <returns></returns>
        IEnumerable<T> SearchByMetadata(string key, string likeValue);

        /// <summary>
        /// Gets game records with exact metadata value match.
        /// </summary>
        /// <param name="key">The metadata key to search for</param>
        /// <param name="exactValue">The exact metadata value match</param>
        /// <returns></returns>
        IEnumerable<T> GetByMetadata(string key, string exactValue);
        
        /// <summary>
        /// Gets all the records in the library
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetRecords();

        /// <summary>
        /// The metadata store associated with this library.
        /// </summary>
        IMetadataStore MetadataStore { get; }
    }
}
