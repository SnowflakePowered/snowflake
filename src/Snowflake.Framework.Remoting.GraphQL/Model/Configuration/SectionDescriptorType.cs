using HotChocolate.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Configuration
{
    public sealed class SectionDescriptorType
        : ObjectType<IConfigurationSectionDescriptor>
    {
        protected override void Configure(IObjectTypeDescriptor<IConfigurationSectionDescriptor> descriptor)
        {
            descriptor.Name("SectionDescriptor")
                .Description("Describes a configuration section.");
            descriptor.Field(s => s.SectionKey)
                .Description("The string that uniquely identifies this section with regard to the parent collection.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(s => s.SectionName)
                .Description("The name of the section as it will be serialized in the configuration file. " +
                "This is not `sectionKey`, and has none of the same guarantees, namely, uniqueness.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(o => o.Description)
                 .Description("Describes the section in detail.")
                 .Type<NonNullType<StringType>>();
            descriptor.Field(o => o.DisplayName)
                .Description("The human readable name of this section.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(o => o.Options)
                .Name("optionDescriptors")
                .Description("Describes the options contained within this configuration section.")
                .Type<NonNullType<ListType<NonNullType<OptionDescriptorType>>>>();

        }
    }
}
