using GraphQL.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.Configuration
{
    public class ConfigurationSectionGraphType : ObjectGraphType<KeyValuePair<string, IConfigurationSection>>
    {
        public ConfigurationSectionGraphType()
        {
            Name = "ConfigurationSection";
            Description = "A configuration section is a grouping of related `ConfigurationOptions`.";
            Field<StringGraphType>("sectionKey",
                description: "The key of this configuration section.",
                resolve: context => context.Source.Key);
            Field<ConfigurationSectionDescriptorGraphType>("descriptor",
                description: "Describes this configuration section.",
                resolve: context => context.Source.Value.Descriptor);
            Field<ListGraphType<ConfigurationValueGraphType>>("values",
                description: "The values of this instance of the configuration.",
                resolve: context => context.Source.Value.Values);
        }
    }
}
