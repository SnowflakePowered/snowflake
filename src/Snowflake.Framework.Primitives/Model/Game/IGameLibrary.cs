using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Game
{
    /// <summary>
    /// Represents an extendable Game library. An instance of <see cref="IGameLibrary"/> is one of the core services
    /// that are always available when Snowflake starts, and handles all information related to games, including
    /// files and configuration.
    /// 
    /// Implements the repository pattern for <see cref="IGame"/>.
    /// </summary>
    public interface IGameLibrary
    {
        /// <summary>
        /// Registers an extension to the game library.
        /// 
        /// Extensions are responsible for their own data, and are created using a factory of type <typeparamref name="TExtensionProvider"/>.
        /// Extensions are local to a given <see cref="IGameRecord"/> that exists in this library, and are responsible for managing their
        /// own data. It is also idiomatic to create an extension method WithExtension to allow fluent access to the extension. 
        /// 
        /// See <see cref="GameFileExtensionExtensions"/> for an example of how such extension methods are implemented.
        /// </summary>
        /// <typeparam name="TExtensionProvider">The type of the extension provider.</typeparam>
        /// <typeparam name="TExtension">The extension instance that is produced for a game.</typeparam>
        /// <param name="extension">An instance of the extension factory.</param>
        void AddExtension<TExtensionProvider, TExtension>(TExtensionProvider extension)
            where TExtensionProvider : IGameExtensionProvider<TExtension>
            where TExtension : class, IGameExtension;

        /// <summary>
        /// Creates a game in the game library.
        /// </summary>
        /// <param name="platformId"></param>
        /// <returns></returns>
        IGame CreateGame(PlatformId platformId);

        /// <summary>
        /// Gets a registered extension provider of the given type.
        /// If no extension of type <typeparamref name="T"/> is 
        /// registered, returns null.
        /// </summary>
        /// <typeparam name="T">The type of the extension provider.</typeparam>
        /// <returns>The regsitered instance of the requested extension provider. </returns>
        T GetExtension<T>() where T : class, IGameExtensionProvider;

        /// <summary>
        /// Gets a game that was created in the library by its unique GUID.
        /// </summary>
        /// <param name="guid">The unique GUID of the game.</param>
        /// <returns>The game with the provided GUID, or null if it does not exist</returns>
        IGame? GetGame(Guid guid);

        /// <summary>
        /// Retrieves all games in the library. See <see cref="GetGames(Expression{Func{IGameRecord, bool}})"/> instead, 
        /// instead of enumerating all possible games.
        /// </summary>
        /// <returns>All games in the library.</returns>
        IEnumerable<IGame> GetAllGames();

        /// <summary>
        /// Retrieves all games in the library that fulfill the provided predicate. The default implementation
        /// executes this predicate in client code. Use <see cref="QueryGames(Expression{Func{IGameRecordQuery, bool}})"/>
        /// instead if possible for more performant queries.
        /// </summary>
        /// <param name="predicate">The predicate to filter on.</param>
        /// <returns>All games in the library that fulfill the provided predicate.</returns>
        IEnumerable<IGame> GetGames(Expression<Func<IGameRecord, bool>> predicate);

        /// <summary>
        /// Retrieves all games in the library that fulfill the provided predicate. The default implementation
        /// executes the predicate on the database than in client code, thus is much faster than 
        /// using <see cref="GetAllGames"/> or <see cref="GetGames(Expression{Func{IGameRecord, bool}})"/>.
        /// </summary>
        /// <param name="predicate">The predicate to filter on.</param>
        /// <returns>All games in the library that fulfill the provided predicate.</returns>
        IEnumerable<IGame> QueryGames(Expression<Func<IGameRecordQuery, bool>> predicate);

        /// <summary>
        /// Retrieves all games in the library that fulfill the provided predicate asynchronously. The default implementation
        /// executes this predicate on the database rather than in client code, thus is much faster than 
        /// using <see cref="GetAllGames"/> or <see cref="GetGames(Expression{Func{IGameRecord, bool}})"/>.
        /// </summary>
        /// <param name="predicate">The predicate to filter on.</param>
        /// <returns>All games in the library that fulfill the provided predicate.</returns>
        Task<IEnumerable<IGame>> QueryGamesAsync(Expression<Func<IGameRecordQuery, bool>> predicate);

        /// <summary>
        /// Updates a <see cref="IGameRecord"/> that exists in the database by providing the changed <see cref="IGameRecord"/>.
        /// </summary>
        /// <param name="game">The updated <see cref="IGameRecord"/></param>
        void UpdateGameRecord(IGameRecord game);
    }
}
