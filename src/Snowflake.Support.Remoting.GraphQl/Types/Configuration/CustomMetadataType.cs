﻿using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Support.Remoting.GraphQl.Types.Values;

namespace Snowflake.Support.Remoting.GraphQl.Types.Configuration
{
    public class CustomMetadataType : ObjectGraphType<KeyValuePair<string, object>>
    {
        public CustomMetadataType()
        {
            Name = "CustomMetadata";
            Description = "Custom metadata for usually flag options.";
            Field(k => k.Key).Description("The custom metadata key.");
            Field<PrimitiveGraphType>("value",
                description: "The value of the custom metadata.",
                resolve: context => context.Source.Value);
            Field<ValueGraphType>("typedValue",
               description: "The value of the custom metadata.",
               resolve: context => context.Source.Value);
        }
    }
}
