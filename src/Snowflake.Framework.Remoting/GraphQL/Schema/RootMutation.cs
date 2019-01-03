using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Relay.Types;
using GraphQL.Types;
using GraphQL.Types.Relay;

namespace Snowflake.Support.Remoting.GraphQL.RootProvider
{
    internal class RootMutation : ObjectGraphType<object>
    {
        public RootMutation()
        {
            this.Name = "Mutation";
            this.Description = "The mutation root of Snowflake's GraphQL interface";
        }
    }
}
