using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Model.FileSystem;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;
using Snowflake.Model.Records.Game;
using Zio;

namespace Snowflake.Model.Game
{
    internal class GameLibrary : IGameLibrary
    {
        IDictionary<Type, (Type extensionType, IGameExtensionProvider extension)> Extensions { get; }

        internal GameLibrary(GameRecordLibrary gamesLibrary)
        {
            this.GameRecordLibrary = gamesLibrary;
            this.Extensions = new Dictionary<Type, (Type, IGameExtensionProvider)>();
        }

        public void AddExtension<TExtensionMaker, TExtension>(TExtensionMaker extension)
            where TExtensionMaker : IGameExtensionProvider<TExtension>
            where TExtension: class, IGameExtension
        {
            this.Extensions.Add(typeof(TExtensionMaker), (typeof(TExtension), extension));
        } 

        public T? GetExtension<T>()
            where T: class, IGameExtensionProvider
        {
            return this.Extensions[typeof(T)].extension as T;
        }

        private GameRecordLibrary GameRecordLibrary { get; }

        public IGame CreateGame(PlatformId platformId)
        {
            var gameRecord = this.GameRecordLibrary.CreateRecord(platformId);
            var extensions = this.Extensions
                .ToDictionary(k => k.Value.extensionType, 
                v => v.Value.extension.MakeExtension(gameRecord));
            return new Game(gameRecord, extensions);
        }

        public void UpdateGameRecord(IGameRecord game)
        {
            this.GameRecordLibrary.UpdateRecord(game);
        }

        public IGame? GetGame(Guid guid)
        {
            var gameRecord = this.GameRecordLibrary.GetRecord(guid);
            if (gameRecord == null) return null;
            var extensions = this.Extensions
                .ToDictionary(k => k.Value.extensionType, v => v.Value.extension.MakeExtension(gameRecord));
            return new Game(gameRecord, extensions); 
        }

        public IEnumerable<IGame> GetGames(Expression<Func<IGameRecord, bool>> predicate)
        {
           return this.GameRecordLibrary.GetRecords(predicate)
           .Select(gameRecord =>
            {
                var extensions = this.Extensions
                .ToDictionary(k => k.Value.extensionType, v => v.Value.extension.MakeExtension(gameRecord));
                return new Game(gameRecord, extensions);
            });
        }

        public IEnumerable<IGame> GetAllGames()
        {
            return this.GameRecordLibrary.GetAllRecords()
            .Select(gameRecord =>
            {
                var extensions = this.Extensions
               .ToDictionary(k => k.Value.extensionType, v => v.Value.extension.MakeExtension(gameRecord));
                return new Game(gameRecord, extensions);
            });
        }

        public async Task<IEnumerable<IGame>> GetGamesAsync(Expression<Func<IGameRecord, bool>> predicate)
        {
            return (await this.GameRecordLibrary.GetRecordsAsync(predicate))
            .Select(gameRecord =>
            {
                var extensions = this.Extensions
               .ToDictionary(k => k.Value.extensionType, v => v.Value.extension.MakeExtension(gameRecord));
                return new Game(gameRecord, extensions);
            });
        }
    }
}
