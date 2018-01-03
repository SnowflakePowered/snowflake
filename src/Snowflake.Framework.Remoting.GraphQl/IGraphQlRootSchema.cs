using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Framework.Remoting.GraphQl.Query;

namespace Snowflake.Framework.Remoting.GraphQl
{
    public interface IGraphQlRootSchema
    {
        void Register(QueryBuilder queries);
    }
}
