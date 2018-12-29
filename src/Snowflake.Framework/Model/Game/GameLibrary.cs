using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Model.FileSystem;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;
using Snowflake.Model.Records.Game;
using Zio;

namespace Snowflake.Model.Game
{
    internal class GameLibrary
    {
        internal GameLibrary(GameRecordLibrary gamesLibrary, FileRecordLibrary fileLibrary, 
            IFileSystem gameFolderFs)
        {
            this.GameRecordLibrary = gamesLibrary;
            this.FileRecordLibrary = fileLibrary;
            this.GameFolderRoot = gameFolderFs;
        }

        private GameRecordLibrary GameRecordLibrary { get; }
        public FileRecordLibrary FileRecordLibrary { get; }
        public IFileSystem GameFolderRoot { get; }

        public IGame CreateGame(PlatformId platformId)
        {
            var gameRecord = this.GameRecordLibrary.CreateRecord(platformId);
            var gameFsRoot = this.GameFolderRoot.GetOrCreateSubFileSystem((UPath)"/" / gameRecord.RecordId.ToString());
            return new Game(gameRecord, gameFsRoot, this.FileRecordLibrary);
        }

        public void UpdateGame(IGameRecord game)
        {
            this.GameRecordLibrary.UpdateRecord(game);
        }

        public void UpdateFile(IFileRecord file)
        {
            this.FileRecordLibrary.UpdateRecord(file);
        }

        public IGame? GetGame(Guid guid)
        {
            var gameRecord = this.GameRecordLibrary.GetRecord(guid);
            if (gameRecord == null) return null;
            var gameFsRoot = this.GameFolderRoot.GetOrCreateSubFileSystem((UPath)"/" / gameRecord.RecordId.ToString());
            return new Game(gameRecord, gameFsRoot, this.FileRecordLibrary);
        }

        public IEnumerable<IGame> GetGames(Expression<Func<IGameRecord, bool>> predicate)
        {
           return this.GameRecordLibrary.GetRecords(predicate)
           .Select(gameRecord =>
            {
               var gameFsRoot = this.GameFolderRoot.GetOrCreateSubFileSystem((UPath)"/" / gameRecord.RecordId.ToString());
                    return new Game(gameRecord, gameFsRoot, this.FileRecordLibrary);
           });
        }
    }
}
