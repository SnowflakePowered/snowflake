using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Configuration;

namespace Snowflake.Support.Remoting.GraphQL.Types.Configuration
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
