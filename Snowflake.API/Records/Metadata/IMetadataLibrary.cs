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
    public interface IMetadataLibrary : ILibrary<IRecordMetadata>
    {

        /// <summary>
        /// Gets all metadata with a specific target guid
        /// </summary>
        /// <param name="target">The element target</param>
        /// <returns>All the metadata for the target element</returns>
        IDictionary<string, IRecordMetadata> GetAllForElement(Guid target);

        /// <summary>
        /// Executes a `LIKE` search on a metadata value
        /// </summary>
        /// <param name="key">The key to search for</param>
        /// <param name="likeValue">The fuzzy search value</param>
        /// <returns>All the metadata with matching key/values</returns>
        IEnumerable<IRecordMetadata> Search(string key, string likeValue);

        /// <summary>
        /// Gets metadata with exact metadata value match.
        /// </summary>
        /// <param name="key">The metadata key to search for</param>
        /// <param name="exactValue">The exact metadata value match</param>
        /// <returns>All the metadata with matching key/values</returns>
        IEnumerable<IRecordMetadata> Get(string key, string exactValue);

        /// <summary>
        /// Gets the keyed metadata for a record
        /// </summary>
        /// <param name="key">The metadata key</param>
        /// <param name="recordId">The metadata record Id</param>
        /// <returns></returns>
        IRecordMetadata Get(string key, Guid recordId);
    }
}
