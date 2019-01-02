using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Game
{
    public interface IGameLibrary
    {
        void AddExtension<TExtensionMaker, TExtension>(TExtensionMaker extension)
            where TExtensionMaker : IGameExtensionProvider<TExtension>
            where TExtension : class, IGameExtension;

        IGame CreateGame(PlatformId platformId);
        T GetExtension<T>() where T : class, IGameExtensionProvider;
        IGame GetGame(Guid guid);
        IEnumerable<IGame> GetAllGames();
        IEnumerable<IGame> GetGames(Expression<Func<IGameRecord, bool>> predicate);
        Task<IEnumerable<IGame>> GetGamesAsync(Expression<Func<IGameRecord, bool>> predicate);
        void UpdateGameRecord(IGameRecord game);
    }
}
