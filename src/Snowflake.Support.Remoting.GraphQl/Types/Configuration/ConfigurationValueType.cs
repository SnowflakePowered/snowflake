using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Support.Remoting.GraphQl.Types.Values;

namespace Snowflake.Support.Remoting.GraphQl.Types.Configuration
{
    public class ConfigurationValueType : ObjectGraphType<IConfigurationValue>
    {
        public ConfigurationValueType()
        {
            Field<GuidGraphType>("guid",
                description: "The GUID of this saved configuration value.",
                resolve: context => context.Source.Guid);
            Field<PrimitiveGraphType>("value",
                description: "The value of this saved configuration value.",
                resolve: context => context.Source.Value);
            Field<ValueGraphType>("typedValue",
                description: "The value of this saved configuration value, typed on unions.",
                resolve: context => context.Source.Value);

        }
    }
}
