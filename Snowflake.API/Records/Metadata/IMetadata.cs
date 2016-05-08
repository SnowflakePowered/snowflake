using System;

namespace Snowflake.Records.Metadata
{
    /// <summary>
    /// Represents a piece of metadata for a game or a file.
    /// </summary>
    public interface IMetadata
    {
        /// <summary>
        /// The key of the metadata
        /// </summary>
        string Key { get; }
        /// <summary>
        /// The value of the metadata
        /// </summary>
        string Value { get; }
        /// <summary>
        /// The guid of the metadata.
        /// A metadata with the same key and element guid should produce the same Guidv3, with the
        /// UUID v3 namespace being the element guid.
        /// </summary>
        Guid MetadataGuid { get; }
        /// <summary>
        /// The guid of the element of the metadata
        /// </summary>
        Guid Element { get; }

    }
}