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
        /// Gets the file library that holds the file information for this game.
        /// </summary>
        IFileLibrary FileLibrary { get; }

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
