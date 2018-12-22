using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Configuration;
using Snowflake.Support.Remoting.GraphQl.Types.Values;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.Configuration
{
    public class ConfigurationValueInputGraphType : ObjectGraphType<IConfigurationValue>
    {
        public ConfigurationValueInputGraphType()
        {
            Name = "RawConfigurationValue";
            Description = "The raw configuration value.";
            Field<GuidGraphType>("guid",
                description: "The GUID of this saved configuration value.",
                resolve: context => context.Source.Guid);
            Field<PrimitiveGraphType>("value",
                description: "The value of this saved configuration value.",
                resolve: context => context.Source.Value);
        }
    }
}
