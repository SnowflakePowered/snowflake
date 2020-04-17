using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Configuration
{
    public sealed class OptionMetadataType
        : ObjectType<KeyValuePair<string, object>>
    {
        protected override void Configure(IObjectTypeDescriptor<KeyValuePair<string, object>> descriptor)
        {
            descriptor.Name("OptionMetadata")
                .Description("Custom metadata for a option descriptor.");
            descriptor.Field(k => k.Key)
                .Description("The metadata key.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(k => k.Value)
                .Description("The metadata value.")
                .Type<AnyType>();
        }
    }
}
