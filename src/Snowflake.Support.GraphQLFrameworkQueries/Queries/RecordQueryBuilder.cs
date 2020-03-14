using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Framework.Remoting.GraphQL.Attributes;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Snowflake.Model.Records.Game;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Inputs.FileRecord;
using Snowflake.Support.GraphQLFrameworkQueries.Inputs.GameRecord;
using Snowflake.Support.GraphQLFrameworkQueries.Inputs.RecordMetadata;
using Snowflake.Support.GraphQLFrameworkQueries.Types.ControllerLayout;
using Snowflake.Support.GraphQLFrameworkQueries.Types.PlatformInfo;
using Snowflake.Support.GraphQLFrameworkQueries.Types.Model;
using Snowflake.Model.Records.File;
using Snowflake.Model.Game.LibraryExtensions;
using System.Linq.Expressions;
using HotChocolate.Types;
using HotChocolate.Types.Filters;
using HotChocolate.Utilities;
using HotChocolate.Language;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries
{
    //public class GameRecordQueryFilter
    //     : FilterInputType<IGameRecordQuery>
    //{
    //    protected override void Configure(IFilterInputTypeDescriptor<IGameRecordQuery> descriptor)
    //    {
    //        descriptor
    //            .BindFieldsExplicitly()
    //            .Filter(t => t.PlatformID)
    //            .BindFiltersExplicitly()
    //            .AllowEquals().And()
    //            .AllowStartsWith().Name("manufacturer");
    //        descriptor
    //            .BindFieldsExplicitly()
    //            .Filter(t => t.RecordID)
    //            .BindFiltersExplicitly()
    //            .AllowEquals();
    //    }
    //}

    //public class 
    public class RecordQueryBuilder : QueryBuilder
    {
        private IGameLibrary GameLibrary { get; }

        public RecordQueryBuilder(IGameLibrary gameLibrary, IStoneProvider stoneProvider)
        {
            this.GameLibrary = gameLibrary;
        }

        public IQueryable<IGame> GetGames(Expression<Func<IGameRecordQuery, bool>> predicate)
        {
            return this.GameLibrary.QueryGames(predicate);
        }

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

        //[Query("game", "Get a game by an ID", typeof(GameGraphType))]
        //[Parameter(typeof(Guid), typeof(GuidGraphType), "guid", "The unique game GUID")]
        //public IGame GetGame(Guid guid)
        //{
        //    return this.GameLibrary.GetGame(guid);
        //}

        //[Connection("gamesByPlatform", "Get games filtered by a given platform", typeof(GameGraphType))]
        //[Parameter(typeof(string), typeof(StringGraphType), "platformId", "The platform ID")]
        //public IEnumerable<IGame> GetGamesByPlatform(string platformId)
        //{
        //    return this.GameLibrary.QueryGames(g => g.PlatformID == platformId);
        //}

        //[Mutation("addGame", "Adds a game to the database directly.", typeof(GameGraphType))]
        //[Parameter(typeof(GameRecordInputObject), typeof(GameRecordInputType), "input", "game input")]
        //public IGame AddGame(GameRecordInputObject input)
        //{
        //    try
        //    {
        //        var game = this.GameLibrary.CreateGame(input.Platform);
        //        game.Record.Title = input.Title;

        //        foreach (var metadata in input.Metadata)
        //        {
        //            game.Record.Metadata.Add(metadata.Key, metadata.Value);
        //        }

        //        this.GameLibrary.UpdateGameRecord(game.Record);
        //        return this.GameLibrary.GetGame(game.Record.RecordID);
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        throw new KeyNotFoundException($"Unable to find platform {input.Platform}.");
        //    }
        //}
    }
}
