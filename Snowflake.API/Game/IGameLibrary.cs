using System.Collections.Generic;

namespace Snowflake.Game
{
    /// <summary>
    /// A database used to store game information
    /// </summary>
    public interface IGameLibrary
    {
        /// <summary>
        /// Add a game to the database
        /// </summary>
        /// <param name="game">The game to add</param>
        void AddGame(IGameInfo game);
        /// <summary>
        /// Removes a game from the database
        /// </summary>
        /// <param name="game"></param>
        void RemoveGame(IGameInfo game);
        /// <summary>
        /// Get a list of all games in the database
        /// </summary>
        /// <returns>A list of all games in the database</returns>
        IEnumerable<IGameInfo> GetAllGames();
        /// <summary>
        /// Gets a game by it's unique id
        /// </summary>
        /// <param name="uuid">The unique id of the game</param>
        /// <returns>The game with the unique id</returns>
        IGameInfo GetGameByUUID(string uuid);
        /// <summary>
        /// Gets a list of games with a certain matching name
        /// </summary>
        /// <param name="nameSearch">The name of the game to search by</param>
        /// <returns>A list of games with matching titles</returns>
        IEnumerable<IGameInfo> GetGamesByName(string nameSearch);
        /// <summary>
        /// Gets all the games for a certain platform
        /// </summary>
        /// <param name="platformId">The platform id to search for</param>
        /// <returns>All games in a certain platform</returns>
        IEnumerable<IGameInfo> GetGamesByPlatform(string platformId);
    }
}
