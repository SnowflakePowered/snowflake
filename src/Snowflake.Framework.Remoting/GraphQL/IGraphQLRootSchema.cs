using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Framework.Remoting.GraphQL.Query;

namespace Snowflake.Framework.Remoting.GraphQL
{
    public interface IGraphQLService
    {
        void Register(QueryBuilder queries);
    }
}
