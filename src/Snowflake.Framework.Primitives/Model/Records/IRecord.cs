using System;

namespace Snowflake.Model.Records
{
    /// <summary>
    /// Represents a class that can have metadata assigned to it.
    /// </summary>
    public interface IRecord
    {
        /// <summary>
        /// Gets the metadata related to this metadata
        /// </summary>
        IMetadataCollection Metadata { get; }

        /// <summary>
        /// Gets the unique ID of the record.
        /// </summary>
        Guid RecordId { get; }
    }
}
