using HotChocolate.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Configuration
{
    internal sealed class ConfigurationValueInput
    {
        public Guid ValueID { get; set; }
        public object Value { get; set; }
    }

    internal sealed class ConfigurationValueInputType
        : InputObjectType<ConfigurationValueInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ConfigurationValueInput> descriptor)
        {
            descriptor.Name(nameof(ConfigurationValueInput));
            descriptor.Field(i => i.ValueID)
                .Name("valueId")
                .Description("The GUID of the configuration value to modify.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.Value)
                .Description("The new value of the configuration value. For enum (selection) values, this should be an integer.")
                .Type<AnyType>();
        }
    }
}
