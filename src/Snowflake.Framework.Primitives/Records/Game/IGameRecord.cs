using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Metadata;

namespace Snowflake.Records.Game
{
    /// <summary>
    /// Represents a game as a collection of <see cref="IRecordMetadata"/> and <see cref="IFileRecord"/>s
    /// </summary>
    public interface IGameRecord : IRecord
    {
        /// <summary>
        /// The Stone platform ID of this record
        /// </summary>
        string PlatformID { get; }

        /// <summary>
        /// The title of the game
        /// (must be equivalent to &quot;game_title&quot; <see cref="IRecord.Metadata"/>)
        /// </summary>
        string Title { get; }

        /// <summary>
        /// All executable files should be mimetype application/romfile-*, 
        /// or application/romfile-*+zip if in zip format.
        /// </summary>
        IList<IFileRecord> Files { get; }
    }
}
