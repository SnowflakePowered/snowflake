using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Configuration;

namespace Snowflake.Support.Remoting.GraphQL.Types.Configuration
{
    public class ConfigurationCollectionGraphType : ObjectGraphType<IConfigurationCollection>
    {
        public ConfigurationCollectionGraphType()
        {
            Name = "ConfigurationCollection";
            Description =
                "A `ConfigurationCollection` is a collection of `ConfigurationSections` for a given emulator.";
            Field<ConfigurationCollectionDescriptorGraphType>("descriptor",
                description: "Describes this configuration collection.",
                resolve: context => context.Source.Descriptor);
            Field<ListGraphType<ConfigurationSectionGraphType>>("sections",
                description: "The various sections of this configuration collection.",
                resolve: context => context.Source);
        }
    }
}
