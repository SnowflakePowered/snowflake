using HotChocolate.Types;
using Snowflake.Model.Records.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Records
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
                .Description("The unique ID of the file record.");
            descriptor.Field("metadata")
               .Description("The metadata associated with this game.")
               .Resolver(ctx => ctx.Parent<IFileRecord>().Metadata.Select(m => m.Value))
               .Type<ListType<RecordMetadataType>>()
               .UseFiltering();
        }
    }
}
