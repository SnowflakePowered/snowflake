using GraphQL.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.Configuration
{
    public class ConfigurationCollectionDescriptorGraphType : ObjectGraphType<IConfigurationCollectionDescriptor>
    {
        public ConfigurationCollectionDescriptorGraphType()
        {
            Name = "ConfigurationCollectionDescriptor";
            Description = "Describes a `ConfigurationCollection` with the keys of each section.";
            Field<ListGraphType<StringGraphType>>("sectionKeys",
                description: "The keys of this section.",
                resolve: context => context.Source.SectionKeys);

        }
    }
}
