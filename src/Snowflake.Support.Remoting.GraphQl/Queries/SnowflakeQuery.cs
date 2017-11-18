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

            var connection = Connection<IGraphType>()
              .Name("platformInfosX")
              .Description("All Stone Platforms.");
            connection.FieldType.Type = typeof(PlatformInfoType);
            connection.Resolve(context => ConnectionUtils.ToConnection(stone.Platforms.Values, context));
              
        }

        private IEnumerable<string> GetValues(ResolveFieldContext<object> context)
        {
            yield return "Hello";
        }
    }
}
