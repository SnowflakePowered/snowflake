using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Filesystem;
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
            where TExtension : class, IGameExtension
        {
            this.Extensions.Add(typeof(TExtensionMaker), (typeof(TExtension), extension));
        }

        public T GetExtension<T>()
            where T : class, IGameExtensionProvider
        {
            if (this.Extensions.TryGetValue(typeof(T), out var ext))
            {
                return (T)ext.extension;
            }
            throw new KeyNotFoundException($"Unable to find extension of type {typeof(T).Name}");
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

        public IQueryable<IGame> GetAllGames()
        {
            return this.GameRecordLibrary.GetAllRecords()
                .Select(gameRecord => new Game(gameRecord,
                    GameLibrary.MakeExtensions(this.Extensions, gameRecord)));
        }

        public async IAsyncEnumerable<IGame> QueryGamesAsync(Expression<Func<IGameRecordQuery, bool>> predicate)
        {
            await foreach (var gameRecord in this.GameRecordLibrary.QueryRecordsAsync(predicate))
            {
                var extensions = this.Extensions
                        .ToDictionary(k => k.Value.extensionType, v => v.Value.extension.MakeExtension(gameRecord));
                yield return new Game(gameRecord, extensions);
            }
        }

        public IQueryable<IGame> QueryGames(Expression<Func<IGameRecordQuery, bool>> predicate)
        {
           return this.GameRecordLibrary.QueryRecords(predicate)
                .Select(gameRecord => new Game(gameRecord, 
                    GameLibrary.MakeExtensions(this.Extensions, gameRecord)));
        }

        static IDictionary<Type, IGameExtension> MakeExtensions(IDictionary<Type, (Type extensionType, IGameExtensionProvider extension)> extensions, 
            IGameRecord gameRecord)
        {
            return extensions.ToDictionary(k => k.Value.extensionType, v => v.Value.extension.MakeExtension(gameRecord));
        }

        public async Task<IGame> CreateGameAsync(PlatformId platformId)
        {
            var gameRecord = await this.GameRecordLibrary.CreateRecordAsync(platformId);
            var extensions = this.Extensions
                .ToDictionary(k => k.Value.extensionType,
                    v => v.Value.extension.MakeExtension(gameRecord));
            return new Game(gameRecord, extensions);
        }

        public async Task<IGame?> GetGameAsync(Guid guid)
        {
            var gameRecord = await this.GameRecordLibrary.GetRecordAsync(guid);
            if (gameRecord == null) return null;
            var extensions = this.Extensions
                .ToDictionary(k => k.Value.extensionType, v => v.Value.extension.MakeExtension(gameRecord));
            return new Game(gameRecord, extensions);
        }

        public async IAsyncEnumerable<IGame> GetAllGamesAsync()
        {
            await foreach (var gameRecord in this.GameRecordLibrary.GetAllRecordsAsync())
            {
                var extensions = this.Extensions
                   .ToDictionary(k => k.Value.extensionType, v => v.Value.extension.MakeExtension(gameRecord));
                yield return new Game(gameRecord, extensions);
            }
        }

        public Task UpdateGameRecordAsync(IGameRecord game)
        {
            return this.GameRecordLibrary.UpdateRecordAsync(game);
        }
    }
}
