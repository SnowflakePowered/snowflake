using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Model.Records.File;

namespace Snowflake.Support.Remoting.GraphQL.Types.Model
{
    public class FileRecordGraphType : ObjectGraphType<IFileRecord>
    {
        public FileRecordGraphType()
        {
            Name = "FileRecord";
            Description = "A record of a file related to a game.";

            //Field(f => f).Description("The path to the file.");

            Field(f => f.MimeType).Description("The file's mimetype.");

            Field<GuidGraphType>("guid",
                description: "The unique ID of the file.",
                resolve: context => context.Source.RecordId);

            Field<StringGraphType>("id",
                description: "The opaque GraphQL unique ID of the file. For caching purposes only.",
                resolve: context => context.Source.RecordId.ToGraphQlUniqueId("FileRecord"));

            Field<ListGraphType<RecordMetadataGraphType>>(
                "metadata",
                description: "A list of metadata related to this file.",
                resolve: context => context.Source.Metadata.Select(m => m.Value));
            Interface<RecordInterface>();

            Field<FileGraphType>(
                "file",
                description: "The file information",
                resolve: context => context.Source.File);
        }
    }
}
