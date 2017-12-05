using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;

namespace Snowflake.Support.Remoting.GraphQl.Framework
{
    public interface IGraphQlRootSchema
    {
        void Register(QueryBuilder queries);
    }
}
