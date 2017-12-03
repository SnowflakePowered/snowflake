using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Records
{
    /// <summary>
    /// Represents a library of records backed by a metadata store
    /// </summary>
    /// <typeparam name="T">A record type</typeparam>
    public interface IRecordLibrary<T> : ILibrary<T> where T : IRecord
    {
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
        /// The metadata store associated with this library.
        /// </summary>
        IMetadataLibrary MetadataLibrary { get; }

        /// <summary>
        /// The name of this library.
        /// </summary>
        string LibraryName { get; }

    }
}
