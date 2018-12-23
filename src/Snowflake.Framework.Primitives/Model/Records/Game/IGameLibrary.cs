using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Model.Game;
using Snowflake.Records.File;

namespace Snowflake.Model.Records.Game
{
    /// <summary>
    /// A database used to store game information
    /// </summary>
    public interface IGameLibrary : ILibrary<IGame>
    {

        /// <summary>
        /// Creates an empty game
        /// </summary>
        /// <returns></returns>
        IGame CreateGame(PlatformId platformId);

        /// <summary>
        /// Gets a list of games with a certain matching name
        /// </summary>
        /// <param name="nameSearch">The name of the game to search by</param>
        /// <returns>A list of games with matching titles</returns>
        IEnumerable<IGame> GetGamesByTitle(string nameSearch);

        /// <summary>
        /// Gets all the games for a certain platform id
        /// </summary>
        /// <param name="platformId">The stone platform id to search for</param>
        /// <returns>All games in a certain platform</returns>
        IEnumerable<IGame> GetGamesByPlatform(PlatformId platformId);
    }
}
