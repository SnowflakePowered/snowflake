﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Filesystem.Library;
using Snowflake.Model.Game;
using Snowflake.Model.Records.File;

namespace Snowflake.Model.Records.Game
{
    /// <summary>
    /// Represents a game as a collection of <see cref="IRecordMetadata"/>.
    /// </summary>
    public interface IGameRecord : IRecord
    {
        /// <summary>
        /// Gets the Stone platform ID of this record
        /// </summary>
        PlatformId PlatformID { get; }

        /// <summary>
        /// Gets or sets the title of the game
        /// (must be equivalent to &quot;game_title&quot; <see cref="IRecord.Metadata"/>)
        /// </summary>
        string? Title { get; set; }
    }
}
