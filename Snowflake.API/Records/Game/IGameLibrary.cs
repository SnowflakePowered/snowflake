using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;

namespace Snowflake.Records.Game
{ 
    /// <summary>
    /// A database used to store game information
    /// </summary>
    public interface IGameLibrary : IRecordLibrary<IGameRecord>
    {

        /// <summary>
        /// The file library that holds the file information for this game.
        /// </summary>
        IFileLibrary FileLibrary { get; }

        /// <summary>
        /// Get a list of all games in the library
        /// </summary>
        /// <returns>A list of all games in the database</returns>
        IEnumerable<IGameRecord> GetGameRecords();
        /// <summary>
        /// Gets a game by it's unique id
        /// </summary>
        /// <param name="uuid">The unique id of the game</param>
        /// <returns>The game with the unique id</returns>
        IGameRecord GetGameByUuid(Guid uuid);
        /// <summary>
        /// Gets a list of games with a certain matching name
        /// </summary>
        /// <param name="nameSearch">The name of the game to search by</param>
        /// <returns>A list of games with matching titles</returns>
        IEnumerable<IGameRecord> GetGamesByTitle(string nameSearch);
        /// <summary>
        /// Gets all the games for a certain platform id
        /// </summary>
        /// <param name="platformId">The stone platform id to search for</param>
        /// <returns>All games in a certain platform</returns>
        IEnumerable<IGameRecord> GetGamesByPlatform(string platformId);
    }
}
