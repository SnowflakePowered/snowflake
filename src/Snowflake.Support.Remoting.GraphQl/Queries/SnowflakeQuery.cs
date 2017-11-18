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
        public SnowflakeQuery()
        {
            this.Name = "Query";
            /*
            Field<PlatformInfoType>(
                 "platformInfo",
                 arguments: new QueryArguments(
                     new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "ID of the Platform" }
                 ),
                 resolve: context => stone.Platforms[context.GetArgument<string>("id")],
                 description: "A Stone Platforms"
               );

            Connection<PlatformInfoType>()
              .Name("platformInfos")
              .Description("All Stone Platforms.")
              .Resolve(context => ConnectionUtils.ToConnection(stone.Platforms.Values, context));
              */
        }
    }
}
