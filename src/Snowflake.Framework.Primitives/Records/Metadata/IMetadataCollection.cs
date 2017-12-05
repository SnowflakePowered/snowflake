using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Records.Metadata
{
    /// <summary>
    /// Represents a collection of metadata
    /// </summary>
    public interface IMetadataCollection : IDictionary<string, IRecordMetadata>
    {
        /// <summary>
        /// Gets the record guid this metadata collection is set to.
        /// </summary>
        Guid Record { get; }

        /// <summary>
        /// The value of this metadata.
        /// If you want to get the raw records, enumerate <see cref="IDictionary{TKey, TValue}.Values"/>
        /// </summary>
        /// <param name="key">The metadata key</param>
        /// <returns>The value of the metadata</returns>
        new string this[string key] { get; set; }

        /// <summary>
        /// Gets a metadata with its guid
        /// </summary>
        /// <param name="guid">The guid of the metadata</param>
        /// <returns>The requested metadata</returns>
        IRecordMetadata this[Guid guid] { get; }

        /// <summary>
        /// Adds a metadata value
        /// </summary>
        /// <param name="recordMetadata">The value to add</param>
        void Add(IRecordMetadata recordMetadata);

        /// <summary>
        /// Adds a record metadata with the specified key and value
        /// </summary>
        /// <param name="key">The key of the metadata</param>
        /// <param name="value">The value of the metadata</param>
        void Add(string key, string value);

        /// <summary>
        /// Copy the metadata of one collection to another, changing the guid of the metadata.
        /// </summary>
        /// <param name="existingMetadata">The existing metadata</param>
        void Add(IDictionary<string, IRecordMetadata> existingMetadata);
    }
}
