using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Records.Metadata
{
    /// <summary>
    /// Represents a database of metadata
    /// </summary>
    public interface IMetadataStore
    {
        /// <summary>
        /// Adds or updates a metadata value
        /// </summary>
        /// <param name="metadata">The metadata to add</param>
        void Set(IMetadata metadata);

        /// <summary>
        /// Removes a metadata value
        /// </summary>
        /// <param name="metadata">The metadata to remove</param>
        void Remove(IMetadata metadata);

        /// <summary>
        /// Removes a metadata value with the given ID
        /// </summary>
        /// <param name="metadataId">The metadata uuid</param>
        void Remove(Guid metadataId);

        /// <summary>
        /// Gets all metadata with a specific target guid
        /// </summary>
        /// <param name="target">The element target</param>
        /// <returns>All the metadata for the target element</returns>
        IDictionary<string, IMetadata> GetAllForElement(Guid target);

        /// <summary>
        /// Executes a `LIKE` search on a metadata value
        /// </summary>
        /// <param name="key">The key to search for</param>
        /// <param name="likeValue">The fuzzy search value</param>
        /// <returns>All the metadata with matching key/values</returns>
        IEnumerable<IMetadata> Search(string key, string likeValue);

        /// <summary>
        /// Gets metadata with exact metadata value match.
        /// </summary>
        /// <param name="key">The metadata key to search for</param>
        /// <param name="exactValue">The exact metadata value match</param>
        /// <returns>All the metadata with matching key/values</returns>
        IEnumerable<IMetadata> GetAll(string key, string exactValue);


        /// <summary>
        /// Gets all metadata in the store
        /// </summary>
        /// <returns>All the metadata currently present</returns>
        IEnumerable<IMetadata> GetAll();

        /// <summary>
        /// Gets a metadata by it's UUID
        /// </summary>
        /// <param name="metadataId">The metadata guid</param>
        /// <returns>The metadata guid</returns>
        IMetadata Get(Guid metadataId);

        /// <summary>
        /// Gets the keyed metadata for a record
        /// </summary>
        /// <param name="key">The metadata key</param>
        /// <param name="recordId">The metadata record Id</param>
        /// <returns></returns>
        IMetadata Get(string key, Guid recordId);
    }
}
