using GraphQL.Types;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Types;
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
            Field<PlatformInfoType>(
              "platformInfo",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the Platform" }
              ),
              resolve: context => stone.Platforms[context.GetArgument<string>("id")]
          );
        }
    }
}
