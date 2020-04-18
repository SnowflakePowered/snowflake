using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Model.Records.Game
{
    /// <summary>
    /// A list of standard keys for game metadata
    /// </summary>
    public static class GameMetadataKeys
    {
        /// <summary>
        /// If game_deleted is "true", the game is deleted. If it 
        /// is not present, or set to any other string, the game
        /// is not deleted.
        /// </summary>
        public static readonly string Deleted = "game_deleted";

        /// <summary>
        /// The platform of the game
        /// </summary>
        public static readonly string Platform = "game_platform";

        /// <summary>
        /// The title of the game
        /// </summary>
        public static readonly string Title = "game_title";

        /// <summary>
        /// A game description
        /// </summary>
        public static readonly string Description = "game_description";

        /// <summary>
        /// The game's region
        /// </summary>
        public static readonly string Region = "game_region";

        /// <summary>
        /// The game's publisher
        /// </summary>
        public static readonly string Publisher = "game_publisher";

        /// <summary>
        /// The game's release date
        /// </summary>
        public static readonly string ReleaseDate = "game_release_date";
    }
}
