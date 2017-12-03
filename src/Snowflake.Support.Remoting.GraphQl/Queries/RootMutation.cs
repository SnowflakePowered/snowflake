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
    public class RootMutation : ObjectGraphType<object>
    {
        public RootMutation()
        {
            this.Name = "Mutation";
            this.Description = "The mutation root of Snowflake's GraphQL interface";
        }
    }
}
