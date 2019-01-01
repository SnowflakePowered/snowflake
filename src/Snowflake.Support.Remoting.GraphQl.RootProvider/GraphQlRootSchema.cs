using System;
using System.Collections.Generic;
using System.Text;
using GraphQL;
using GraphQL.Types;
using Snowflake.Configuration;
using Snowflake.Framework.Remoting.GraphQl;
using Snowflake.Framework.Remoting.GraphQl.Query;
using Snowflake.Loader;
using Snowflake.Services;

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
