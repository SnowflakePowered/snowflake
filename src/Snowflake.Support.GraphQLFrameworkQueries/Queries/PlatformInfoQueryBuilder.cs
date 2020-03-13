using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Snowflake.Framework.Remoting.GraphQL.Attributes;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Model.Game;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Types.PlatformInfo;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries
{
    public class PlatformInfoQueryBuilder : QueryBuilder
    {
        private IStoneProvider StoneProvider { get; }

        public PlatformInfoQueryBuilder(IStoneProvider stoneProvider)
        {
            this.StoneProvider = stoneProvider;
        }

        public IEnumerable<IPlatformInfo> GetPlatforms()
        {
            return this.StoneProvider.Platforms.Values;
        }
    }
}
