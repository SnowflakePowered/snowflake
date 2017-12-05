﻿using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Records.Metadata;

namespace Snowflake.Support.Remoting.GraphQl.Types.Record
{
    public class RecordMetadataGraphType : ObjectGraphType<IRecordMetadata>
    {
        public RecordMetadataGraphType()
        {
            Name = "RecordMetadata";
            Description = "A piece of metadata associated with a record.";
            Field<GuidGraphType>("guid",
                       description: "The unique ID of the metadata.",
                       resolve: context => context.Source.Guid);
            Field<GuidGraphType>("record",
                      description: "The GUID of the record this metadata is referenced to.",
                      resolve: context => context.Source.Record);
            Field(m => m.Key).Description("The key identifying this metadata.");
            Field(m => m.Value).Description("The value of this metadata.");
        }
    }
}
