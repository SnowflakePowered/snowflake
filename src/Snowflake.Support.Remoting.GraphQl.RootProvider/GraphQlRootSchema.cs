using System;
using System.Collections.Generic;
using System.Text;
using GraphQL;
using GraphQL.Types;
using Snowflake.Configuration;
using Snowflake.Loader;
using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;

namespace Snowflake.Support.Remoting.GraphQl.RootProvider
{
    class GraphQlRootSchema : Schema, IGraphQlRootSchema
    {
        public GraphQlRootSchema(RootQuery query, RootMutation mutation)
        {
            this.Query = query;
            this.Mutation = mutation;
        }

        /// <inheritdoc/>
        public void Register(QueryBuilder queries)
        {
            queries.RegisterConnectionQueries((RootQuery)this.Query);
            queries.RegisterFieldQueries((RootQuery)this.Query);
            queries.RegisterMutationQueries((RootMutation)this.Mutation);
        }
    }
}
