using HotChocolate.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Configuration
{
    public sealed class ConfigurationValueType
        : ObjectType<IConfigurationValue>
    {
        protected override void Configure(IObjectTypeDescriptor<IConfigurationValue> descriptor)
        {
            descriptor.Name("ConfigurationValue")
                .Description("Represents a single unit of configuration.");
            descriptor.Field(v => v.Value)
                .Description("The value set for the configuration option.")
                .Type<AnyType>();
            descriptor.Field(v => v.Guid)
                .Name("valueID")
                .Description("The GUID that refers to this specific value.")
                .Type<UuidType>();
        }
    }
}
