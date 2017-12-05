using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQl.Framework.Query
{
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
