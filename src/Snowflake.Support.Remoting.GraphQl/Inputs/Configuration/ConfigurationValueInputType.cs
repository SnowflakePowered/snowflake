using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Support.Remoting.GraphQl.Types.Values;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.Configuration
{
    public class ConfigurationValueInputType : InputObjectGraphType<ConfigurationValueInputObject>
    {
        public ConfigurationValueInputType()
        {
            Field<GuidGraphType>("guid",
                description: "The GUID of the value to set.",
                resolve: context => context.Source.Guid);

            Field<PrimitiveGraphType>("value",
                description: "The value to set the option to.",
                resolve: context => context.Source.Value);
        }
    }
}
