using Snowflake.Support.Remoting.GraphQl.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Framework.Query
{
    public abstract partial class QueryBuilder
    {
        /// <summary>
        /// Register field queries of this class with the given root query.
        /// </summary>
        /// <param name="root"></param>
        public void RegisterFieldQueries(RootQuery root)
        {
            var fieldQueries = this.EnumerateFieldQueries();
            foreach (var query in fieldQueries)
            {
                var madeQuery = this.MakeFieldQuery(query.fieldMethod, query.fieldAttr, query.paramAttr);
                this.RegisterQuery(madeQuery, root);
            }
        }

        public void RegisterConnectionQueries(RootQuery root)
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
