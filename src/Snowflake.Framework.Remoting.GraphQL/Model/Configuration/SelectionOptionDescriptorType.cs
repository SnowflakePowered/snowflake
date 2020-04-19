using HotChocolate.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Configuration
{
    public sealed class SelectionOptionDescriptorType
        : ObjectType<ISelectionOptionDescriptor>
    {
        protected override void Configure(IObjectTypeDescriptor<ISelectionOptionDescriptor> descriptor)
        {
            descriptor.Name("SelectionOptionDescriptor")
                .Description("Describes one selection out of many for a selection (enumerated) option.");
            descriptor.Field(o => o.DisplayName)
                .Description("The friendly display name of the option.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(o => o.Private)
                .Description("Whether or not this selection should be visible by default to the user.")
                .Type<NonNullType<BooleanType>>();
            descriptor.Field(o => o.EnumName)
                .Description("The name of the backing enumeration value this selection is represented by.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(o => o.NumericValue)
                .Description("The numeric value that represents this selection in the backing enumeration.")
                .Type<NonNullType<IntType>>();
            descriptor.Field(o => o.SerializeAs)
                .Description("The value that this selection will be serialized into when serializing the configuration file.")
                .Type<NonNullType<StringType>>();
        }
    }
}
