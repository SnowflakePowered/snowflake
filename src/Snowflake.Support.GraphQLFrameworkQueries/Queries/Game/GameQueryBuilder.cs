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
    public class GameQueryBuilder : QueryBuilder
    {
        private IGameLibrary GameLibrary { get; }

        public GameQueryBuilder(IGameLibrary gameLibrary)
        {
            this.GameLibrary = gameLibrary;
        }

        public IQueryable<IGame> GetGames(Expression<Func<IGameRecordQuery, bool>> predicate)
        {
            return this.GameLibrary.QueryGames(predicate);
        }

        public IQueryable<IGame> GetAllGames()
        {
            return this.GameLibrary.GetAllGames();
            //return this.GameLibrary.QueryGames(predicate);
        }
    }
}
