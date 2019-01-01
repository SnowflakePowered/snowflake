using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Framework.Remoting.GraphQl.Attributes;
using Snowflake.Framework.Remoting.GraphQl.Query;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Snowflake.Model.Records.Game;
using Snowflake.Platform;

using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Inputs.FileRecord;
using Snowflake.Support.Remoting.GraphQl.Inputs.GameRecord;
using Snowflake.Support.Remoting.GraphQl.Inputs.RecordMetadata;
using Snowflake.Support.Remoting.GraphQl.Types.ControllerLayout;
using Snowflake.Support.Remoting.GraphQl.Types.PlatformInfo;
using Snowflake.Support.Remoting.GraphQl.Types.Model;

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

        [Connection("games", "Get all Games", typeof(GameGraphType))]
        public IEnumerable<IGame> GetGames()
        {
            return this.GameLibrary.GetAllGames();
        }

        //[Connection("files", "Get all files", typeof(FileRecordGraphType))]
        //public IEnumerable<IFileRecord> GetFiles()
        //{
        //    return this.GameLibrary.FileLibrary.GetAllRecords();
        //}

        //[Field("file", "Get a file by an ID", typeof(FileRecordGraphType))]
        //[Parameter(typeof(Guid), typeof(GuidGraphType), "guid", "The unique file GUID")]
        //public IFileRecord GetFile(Guid guid)
        //{
        //    return this.GameLibrary.FileLibrary.Get(guid);
        //}

        //[Field("fileByPath", "Get a file by a filepath", typeof(FileRecordGraphType))]
        //[Parameter(typeof(string), typeof(StringGraphType), "filePath", "The path of the file on the FileSystem")]
        //public IFileRecord GetFileByPath(string filePath)
        //{
        //    var path = this.GameLibrary.FileLibrary.Get(filePath);
        //    if (path == null)
        //    {
        //        throw new FileNotFoundException($"The record for ${filePath} was not found in the database.");
        //    }

        //    return path;
        //}

        //[Field("game", "Get a game by an ID", typeof(GameRecordGraphType))]
        //[Parameter(typeof(Guid), typeof(GuidGraphType), "guid", "The unique game GUID")]
        //public IGameRecord GetGame(Guid guid)
        //{
        //    return this.GameLibrary.Get(guid);
        //}

        //[Connection("gamesByPlatform", "Get games filtered by a given platform", typeof(GameRecordGraphType))]
        //[Parameter(typeof(string), typeof(StringGraphType), "platformId", "The platform ID")]
        //public IEnumerable<IGameRecord> GetGamesByPlatform(string platformId)
        //{
        //    return this.GameLibrary.GetGamesByPlatform(platformId);
        //}

        [Mutation("addGame", "Adds a game to the database directly.", typeof(GameGraphType))]
        [Parameter(typeof(GameRecordInputObject), typeof(GameRecordInputType), "input", "game input")]
        public IGame AddGame(GameRecordInputObject input)
        {
            try
            {
                var game = this.GameLibrary.CreateGame(input.Platform);
                game.Record.Title = input.Title;
               
                foreach (var metadata in input.Metadata)
                {
                   game.Record.Metadata.Add(metadata.Key, metadata.Value);
                }

                this.GameLibrary.UpdateGameRecord(game.Record);
                return this.GameLibrary.GetGame(game.Record.RecordId);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Unable to find platform {input.Platform}.");
            }
        }

        //[Mutation("addFile", "Adds a file to the database directly.", typeof(FileRecordGraphType))]
        //[Parameter(typeof(FileRecordInputObject), typeof(FileRecordInputType), "input", "File input")]
        //public IFileRecord AddFile(FileRecordInputObject input)
        //{
        //    var file = new FileRecord(input.FilePath, input.MimeType);
        //    this.GameLibrary.FileLibrary.Set(file);
        //    return file;
        //}
    }
}
