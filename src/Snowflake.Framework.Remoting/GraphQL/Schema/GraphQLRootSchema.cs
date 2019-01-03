using System;
using System.Collections.Generic;
using System.Text;
using GraphQL;
using GraphQL.Types;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Framework.Remoting.GraphQL.Query;


namespace Snowflake.Support.Remoting.GraphQL.RootProvider
{
    class GraphQlRootSchema : Schema, IGraphQLService
    {
        public GraphQlRootSchema(RootQuery query, RootMutation mutation)
        {
            this.Query = query;
            this.Mutation = mutation;
        }

        /// <inheritdoc/>
        public void Register(QueryBuilder queries)
        {
            queries.RegisterConnectionQueries((RootQuery) this.Query);
            queries.RegisterFieldQueries((RootQuery) this.Query);
            queries.RegisterMutationQueries((RootMutation) this.Mutation);
        }
    }
}
