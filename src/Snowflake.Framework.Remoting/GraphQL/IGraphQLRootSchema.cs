using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Remoting.GraphQL.Query;

namespace Snowflake.Remoting.GraphQL
{
    /// <summary>
    /// Provides access to the GraphQL root schema.
    /// </summary>
    public interface IGraphQLService
    {
        /// <summary>
        /// Registers a <see cref="QueryBuilder"/> into the root schema.
        /// </summary>
        /// <param name="queries">The <see cref="QueryBuilder"/> instance implements the queries.</param>
        void Register(QueryBuilder queries);
    }
}
