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
    /// Represents a game as a collection of <see cref="IMetadata"/> and <see cref="IFileRecord"/>s
    /// </summary>
    public interface IGameRecord : IMetadataAssignable
    {
        /// <summary>
        /// The Stone platform ID of this record
        /// </summary>
        string PlatformId { get; }

        /// <summary>
        /// The title of the game
        /// (must be equivalent to &quot;game_title&quot; <see cref="IMetadataAssignable.Metadata"/>)
        /// </summary>
        string Title { get; }

        /// <summary>
        /// All executable files should be mimetype application/romfile-*, 
        /// or application/romfile-*+zip if in zip format.
        /// </summary>
        IEnumerable<IFileRecord> GameFiles { get; }
        

    }
}
