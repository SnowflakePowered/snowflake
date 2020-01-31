using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Framework.Remoting.GraphQL.Query;

namespace Snowflake.Framework.Remoting.GraphQL
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
