using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Input.Controller;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Inputs.FileRecord;
using Snowflake.Support.Remoting.GraphQl.Inputs.GameRecord;
using Snowflake.Support.Remoting.GraphQl.Types.ControllerLayout;
using Snowflake.Support.Remoting.GraphQl.Types.PlatformInfo;
using Snowflake.Support.Remoting.GraphQl.Types.Record;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class RecordQueryBuilder : QueryBuilder
    {
        private IGameLibrary GameLibrary { get; }
        private IStoneProvider StoneProvider { get; }
        public RecordQueryBuilder(IGameLibrary gameLibrary, IStoneProvider stoneProvider)
        {
            this.GameLibrary = gameLibrary;
            this.StoneProvider = stoneProvider;
        }

        [Connection("games", "Get all Games", typeof(GameRecordType))]
        public IEnumerable<IGameRecord> GetGames()
        {
            return this.GameLibrary.GetAllRecords();
        }

        [Connection("files", "Get all files", typeof(FileRecordType))]
        public IEnumerable<IFileRecord> GetFiles()
        {
            return this.GameLibrary.FileLibrary.GetAllRecords();
        }


        [Field("file", "Get a file by an ID", typeof(FileRecordType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "guid", "The unique file GUID")]
        public IFileRecord GetFile(Guid guid)
        {
            return this.GameLibrary.FileLibrary.Get(guid);
        }

        [Field("fileByPath", "Get a file by a filepath", typeof(FileRecordType))]
        [Parameter(typeof(string), typeof(StringGraphType), "filePath", "The path of the file on the FileSystem")]
        public IFileRecord GetFileByPath(string filePath)
        {
            var path = this.GameLibrary.FileLibrary.Get(filePath);
            if (path == null)
            {
                throw new FileNotFoundException($"The record for ${filePath} was not found in the database.");
            }
            return path;
        }

        [Field("game", "Get a game by an ID", typeof(GameRecordType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "guid", "The unique game GUID")]
        public IGameRecord GetGame(Guid guid)
        {
            return this.GameLibrary.Get(guid);
        }

        [Connection("gamesByPlatform", "Get games filtered by a given platform", typeof(GameRecordType))]
        public IEnumerable<IGameRecord> GetGamesByPlatform(string platformId)
        {
            return this.GameLibrary.GetGamesByPlatform(platformId);
        }

        [Mutation("addGame", "Adds a game to the database directly.", typeof(StringGraphType))]
        [Parameter(typeof(GameRecordInputObject), typeof(GameRecordInputType), "gameObject", "game input")]
        public IGameRecord AddGame(GameRecordInputObject gameObject)
        {
            try
            {
                var platform = this.StoneProvider.Platforms[gameObject.Platform];
                var game = new GameRecord(platform, gameObject.Title);
                foreach (var metadata in gameObject.Metadata)
                {
                    game.Metadata.Add(metadata.Key, metadata.Value);
                }
                return game;
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Unable to find platform {gameObject.Platform}.");
            }
        }

        [Mutation("addFile", "Adds a file to the database directly.", typeof(StringGraphType))]
        [Parameter(typeof(FileRecordInputObject), typeof(FileRecordInputType), "fileObject", "File input")]
        public IFileRecord AddFile(FileRecordInputObject fileObject)
        {
            var file = new FileRecord(fileObject.FilePath, fileObject.MimeType, fileObject.Record);
            this.GameLibrary.FileLibrary.Set(file);
            return file;
        }
    }
}
