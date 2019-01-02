using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Model.Game;

namespace Snowflake.Model.Records.Game
{
    /// <summary>
    /// Represents a game as a collection of <see cref="IRecordMetadata"/> and <see cref="IFileRecord"/>s
    /// </summary>
    public interface IGameRecord : IRecord
    {
        /// <summary>
        /// Gets the Stone platform ID of this record
        /// </summary>
        PlatformId PlatformId { get; }

        /// <summary>
        /// Gets or sets the title of the game
        /// (must be equivalent to &quot;game_title&quot; <see cref="IRecord.Metadata"/>)
        /// </summary>
        string? Title { get; set; }
    }
}
