using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Framework.Remoting.GraphQL.Attributes;

namespace Snowflake.Framework.Remoting.GraphQL.Query
{
    /// <summary>
    /// A class that provides GraphQL Schema access through <see cref="IGraphQLService.Register(QueryBuilder)"/> must
    /// inherit from <see cref="QueryBuilder"/>. While no extra properties are made visible, <see cref="QueryBuilder"/>
    /// uses some behind-the-scenes reflection access to rewrite properly marked methods of the inheriting class into
    /// GraphQL-accessible functions.
    /// <br/>
    /// See <see cref="FieldAttribute"/>, <see cref="ConnectionAttribute"/>, and <see cref="MutationAttribute"/> for
    /// how to mark a method as a GraphQL query.
    /// </summary>
    public abstract partial class QueryBuilder
    {
        /// <summary>
        /// Register field queries of this class with the given root query.
        /// </summary>
        /// <param name="root"></param>
        internal void RegisterFieldQueries(ObjectGraphType<object> root)
        {
            var fieldQueries = this.EnumerateFieldQueries();
            foreach (var query in fieldQueries)
            {
                var madeQuery = this.MakeFieldQuery(query.fieldMethod, query.fieldAttr, query.paramAttr);
                this.RegisterQuery(madeQuery, root);
            }
        }

        /// <summary>
        /// Register mutation field queries of this class with the given root query.
        /// </summary>
        /// <param name="root"></param>
        internal void RegisterMutationQueries(ObjectGraphType<object> root)
        {
            var fieldQueries = this.EnumerateMutationQueries();
            foreach (var query in fieldQueries)
            {
                var madeQuery = this.MakeMutationQuery(query.fieldMethod, query.fieldAttr, query.paramAttr);
                this.RegisterQuery(madeQuery, root);
            }
        }

        internal void RegisterConnectionQueries(ObjectGraphType<object> root)
        {
            var fieldQueries = this.EnumerateConnectionQueries();
            foreach (var query in fieldQueries)
            {
                var madeQuery = this.MakeConnectionQuery(query.fieldMethod, query.connectionAttr, query.paramAttr);
                this.RegisterQuery(madeQuery, root);
            }
        }
    }
}
