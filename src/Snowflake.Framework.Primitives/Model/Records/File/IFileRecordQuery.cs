using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Model.Records.File
{
    /// <summary>
    /// Represents the queryable view of an <see cref="IFileRecord"/> within an 
    /// <see cref="IGameLibrary"/>.
    /// </summary>
    public interface IFileRecordQuery
    {
        /// <summary>
        /// Gets the mimetype of the file record.
        /// </summary>
        string Mimetype { get; }

        /// <summary>
        /// The RecordID of the file record.
        /// </summary>
        Guid RecordID { get; }

        /// <summary>
        /// Queries the metadata associated with this record.
        /// </summary>
        IEnumerable<IRecordMetadataQuery> Metadata { get; }
    }
}
