using HotChocolate;
using HotChocolate.Types;
using Snowflake.Model.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Records
{
    public sealed class RecordMetadataType
        : ObjectType<IRecordMetadata>
    {
        protected override void Configure(IObjectTypeDescriptor<IRecordMetadata> descriptor)
        {
            descriptor.Name("RecordMetadata")
                .Description("A piece of metadata associated with a record");

            descriptor.Field(r => r.Guid)
                .Name("metadataId")
                .Description("The unique GUID of the metadata.");

            descriptor.Field(r => r.Record)
                .Description("The GUID of the record this metadata is referenced to.");

            descriptor.Field(r => r.Key)
                .Description("The key identifying the content type of this metadata.");

            descriptor.Field(m => m.Value).Description("The value of this metadata.");
        }
    }
}
