using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Configuration;

namespace Snowflake.Support.Remoting.GraphQl.Types.Configuration
{
    public class ConfigurationSectionDescriptorGraphType : ObjectGraphType<IConfigurationSectionDescriptor>
    {
        public ConfigurationSectionDescriptorGraphType()
        {
            Name = "ConfigurationSectionDescriptor";
            Description = "Describes a `ConfigurationSection` with its human readable name and description.";
            Field(s => s.Description).Description("The description of this configuration section.");
            Field(s => s.DisplayName).Description("The human readable name of this configuration section.");
            Field(s => s.SectionName).Description("The section name as it appears in serialized configuration.");
            Field<ListGraphType<ConfigurationOptionDescriptorGraphType>>("options",
                description: "The options of this configuration section.",
                resolve: context => context.Source.Options);
        }
    }
}
