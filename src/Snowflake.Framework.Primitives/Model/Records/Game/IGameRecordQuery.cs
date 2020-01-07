using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Model.Records.Game
{
    /// <summary>
    /// Represents the queryable view of an <see cref="IGameRecord"/> within an 
    /// <see cref="IGameLibrary"/>.
    /// </summary>
    public interface IGameRecordQuery
    {
        /// <summary>
        /// Gets the Stone platform ID of this record
        /// </summary>
        PlatformId PlatformID { get; }
        /// <summary>
        /// The RecordID of the Game Record
        /// </summary>
        Guid RecordID { get; }
        /// <summary>
        /// Queries the metadata associated with this record.
        /// </summary>
        IEnumerable<IRecordMetadataQuery> Metadata { get; }
    }
}
