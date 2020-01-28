using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Configuration;
using Snowflake.Support.GraphQLFrameworkQueries.Types.Values;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.Configuration
{
    public class ConfigurationValueGraphType : ObjectGraphType<KeyValuePair<string, IConfigurationValue>>
    {
        public ConfigurationValueGraphType()
        {
            Name = "ConfigurationValue";
            Description = "The value of a single configuration option key.";
            Field<StringGraphType>("optionKey",
                description: "The key of the Configuration option this value maps for.",
                resolve: context => context.Source.Key);
            Field<GuidGraphType>("guid",
                description: "The GUID of this saved configuration value.",
                resolve: context => context.Source.Value.Guid);
            Field<StringGraphType>("id",
                description: "The opaque GraphQL unique ID of this ConfigurationValue. For caching purposes only.",
                resolve: context => context.Source.Value.Guid.ToGraphQlUniqueId("ConfigurationValue"));
            Field<PrimitiveGraphType>("value",
                description: "The value of this saved configuration value.",
                resolve: context => context.Source.Value.Value);
            Field<ValueGraphType>("typedValue",
                description: "The value of this saved configuration value, typed on unions.",
                resolve: context => context.Source.Value.Value);
        }
    }
}
