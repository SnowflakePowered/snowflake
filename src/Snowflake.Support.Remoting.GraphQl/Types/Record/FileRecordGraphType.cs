using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Records.File;

namespace Snowflake.Support.Remoting.GraphQl.Types.Record
{
    public class FileRecordGraphType : ObjectGraphType<IFileRecord>
    {
        public FileRecordGraphType()
        {
            Name = "FileRecord";
            Description = "A record of a file related to a game or other metadatable record.";
            Field(f => f.FilePath).Description("The path to the file.");
            Field(f => f.MimeType).Description("The file's mimetype.");
            Field<GuidGraphType>("guid",
              description: "The unique ID of the file.",
              resolve: context => context.Source.Guid);
            Field<StringGraphType>("id",
              description: "The opaque GraphQL unique ID of the file. For caching purposes only.",
              resolve: context => context.Source.Guid.ToGraphQlUniqueId("FileRecord"));
            Field<ListGraphType<RecordMetadataGraphType>>(
                "metadata",
                description: "A list of metadata related to this file.",
                resolve: context => context.Source.Metadata.Select(m => m.Value));
            Interface<RecordInterface>();
        }
    }
}
