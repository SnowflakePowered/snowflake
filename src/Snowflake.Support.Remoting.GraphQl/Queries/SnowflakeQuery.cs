using GraphQL.Relay.Types;
using GraphQL.Types;
using GraphQL.Types.Relay;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Types.PlatformInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class SnowflakeQuery: ObjectGraphType<object>
    {
        public SnowflakeQuery(IStoneProvider stone)
        {
            this.Name = "Query"; 
        }

        private IEnumerable<string> GetValues(ResolveFieldContext<object> context)
        {
            yield return "Hello";
        }
    }
}
