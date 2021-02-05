using HotChocolate.Types;
using Snowflake.Model.Records.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Records
{
    public sealed class FileRecordType
        : ObjectType<IFileRecord>
    {
        protected override void Configure(IObjectTypeDescriptor<IFileRecord> descriptor)
        {
            descriptor.Name("FileRecord")
                .BindFieldsExplicitly();
            descriptor.Field(f => f.MimeType)
                .Description("The known mimetype of the file.");
            
            descriptor.Field(f => f.RecordID)
                .Name("fileId")
                .Description("The unique ID of the file record. This is the same as the `fileId` from a ContextualFile");
            descriptor.Field("metadata")
               .Description("The metadata associated with this game.")
               .Resolve(ctx => ctx.Parent<IFileRecord>().Metadata.Select(m => m.Value))
               .Type<ListType<RecordMetadataType>>()
               .UseFiltering();
        }
    }
}
