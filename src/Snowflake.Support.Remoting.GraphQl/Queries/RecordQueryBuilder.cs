using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Input.Controller;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Inputs.Record;
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
        public RecordQueryBuilder(IGameLibrary gameLibrary)
        {
            this.GameLibrary = gameLibrary;
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

        [Mutation("addGame", "Get a game by an ID", typeof(StringGraphType))]
        [Parameter(typeof(GameRecordInputObject), typeof(GameRecordInputType), "gameObject", "game input")]
        public string AddGame(GameRecordInputObject gameObject)
        {
            return gameObject.Metadata[0].Value;
        }
    }
}
