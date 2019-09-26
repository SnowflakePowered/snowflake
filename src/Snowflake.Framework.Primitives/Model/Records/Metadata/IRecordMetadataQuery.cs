using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Snowflake.Model.Records
{
    /// <summary>
    /// Represents a queryable view over metadata within a <see cref="IGameLibrary"/>
    /// </summary>
    public interface IRecordMetadataQuery
    {
        /// <summary>
        /// The metadata key
        /// </summary>
        string MetadataKey { get; }
        /// <summary>
        /// The value of the metadata.
        /// </summary>
        string MetadataValue { get; }
        /// <summary>
        /// Gets the unique ID of the record this metadata belongs to.
        /// </summary>
        Guid RecordID { get; }
        /// <summary>
        /// Gets the unique ID of the metadata.
        /// </summary>
        Guid RecordMetadataID { get; }
    }
}