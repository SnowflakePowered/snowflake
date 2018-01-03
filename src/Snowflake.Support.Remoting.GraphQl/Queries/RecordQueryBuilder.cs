using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Framework.Remoting.GraphQl.Attributes;
using Snowflake.Framework.Remoting.GraphQl.Query;
using Snowflake.Input.Controller;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Inputs.FileRecord;
using Snowflake.Support.Remoting.GraphQl.Inputs.GameRecord;
using Snowflake.Support.Remoting.GraphQl.Inputs.RecordMetadata;
using Snowflake.Support.Remoting.GraphQl.Types.ControllerLayout;
using Snowflake.Support.Remoting.GraphQl.Types.PlatformInfo;
using Snowflake.Support.Remoting.GraphQl.Types.Record;

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

        [Connection("games", "Get all Games", typeof(GameRecordGraphType))]
        public IEnumerable<IGameRecord> GetGames()
        {
            return this.GameLibrary.GetAllRecords();
        }

        [Connection("files", "Get all files", typeof(FileRecordGraphType))]
        public IEnumerable<IFileRecord> GetFiles()
        {
            return this.GameLibrary.FileLibrary.GetAllRecords();
        }

        [Field("file", "Get a file by an ID", typeof(FileRecordGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "guid", "The unique file GUID")]
        public IFileRecord GetFile(Guid guid)
        {
            return this.GameLibrary.FileLibrary.Get(guid);
        }

        [Field("fileByPath", "Get a file by a filepath", typeof(FileRecordGraphType))]
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

        [Field("game", "Get a game by an ID", typeof(GameRecordGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "guid", "The unique game GUID")]
        public IGameRecord GetGame(Guid guid)
        {
            return this.GameLibrary.Get(guid);
        }

        [Connection("gamesByPlatform", "Get games filtered by a given platform", typeof(GameRecordGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "platformId", "The platform ID")]
        public IEnumerable<IGameRecord> GetGamesByPlatform(string platformId)
        {
            return this.GameLibrary.GetGamesByPlatform(platformId);
        }

        [Mutation("addGame", "Adds a game to the database directly.", typeof(GameRecordGraphType))]
        [Parameter(typeof(GameRecordInputObject), typeof(GameRecordInputType), "input", "game input")]
        public IGameRecord AddGame(GameRecordInputObject input)
        {
            try
            {
                var platform = this.StoneProvider.Platforms[input.Platform];
                var game = new GameRecord(platform, input.Title);
                foreach (var metadata in input.Metadata)
                {
                    game.Metadata.Add(metadata.Key, metadata.Value);
                }

                this.GameLibrary.Set(game);
                return this.GameLibrary.Get(game.Guid);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Unable to find platform {input.Platform}.");
            }
        }

        [Mutation("addFile", "Adds a file to the database directly.", typeof(FileRecordGraphType))]
        [Parameter(typeof(FileRecordInputObject), typeof(FileRecordInputType), "input", "File input")]
        public IFileRecord AddFile(FileRecordInputObject input)
        {
            var file = new FileRecord(input.FilePath, input.MimeType);
            this.GameLibrary.FileLibrary.Set(file);
            return file;
        }
    }
}
