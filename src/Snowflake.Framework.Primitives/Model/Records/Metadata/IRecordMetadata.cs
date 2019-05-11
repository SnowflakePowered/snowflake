using System;

namespace Snowflake.Model.Records
{
    /// <summary>
    /// Represents a piece of metadata for a game or a file.
    /// A record metadata is equal to another instance if the key and record guid are the same,
    /// or if otherwise the metadata guid are the same.
    /// <br/>
    /// A record metadata with the same key and record guid should produce the same guid every time,
    /// equality is not dependent on the value of the metadata.
    /// </summary>
    public interface IRecordMetadata
    {
        /// <summary>
        /// Gets the key of the metadata
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets the value of the metadata
        /// </summary>
        string? Value { get; }

        /// <summary>
        /// Gets the guid of the metadata.
        /// A metadata with the same key and element guid should produce the same Guidv3, with the
        /// UUID v3 namespace being the element guid.
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// Gets the guid of the element of the metadata
        /// </summary>
        Guid Record { get; }
    }
}
