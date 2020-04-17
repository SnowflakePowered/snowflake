using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Support.GraphQLFrameworkQueries.Types.Values;

namespace Snowflake.Support.GraphQLFrameworkQueries.Inputs.Configuration
{
    public class ConfigurationValueInputType : InputObjectGraphType<ConfigurationValueInputObject>
    {
        public ConfigurationValueInputType()
        {
            Name = "ConfigurationValueInput";
            Field<GuidGraphType>("guid",
                description: "The GUID of the value to set.",
                resolve: context => context.Source.Guid);

            Field<PrimitiveGraphType>("value",
                description: "The value to set the option to.",
                resolve: context => context.Source.Value);
        }
    }
}
