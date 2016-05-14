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
        /// The record this metadata collection is set to.
        /// </summary>
        Guid Record { get; }

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
        /// Gets a metadata with its guid
        /// </summary>
        /// <param name="guid">The guid of the metadata</param>
        /// <returns>The requested metadata</returns>
        IRecordMetadata this[Guid guid] { get; set; }

    }
}
