using GraphQL;
using GraphQL.Types;
using Snowflake.Configuration;
using Snowflake.Loader;
using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl
{
    class SnowflakeSchema : Schema
    {
        public SnowflakeSchema(RootQuery query)
        {
            this.Query = query;
        }
    }
}
