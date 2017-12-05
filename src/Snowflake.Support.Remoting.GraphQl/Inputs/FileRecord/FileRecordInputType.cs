﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Support.Remoting.GraphQl.Inputs.RecordMetadata;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.FileRecord
{
    public class FileRecordInputType : InputObjectGraphType<FileRecordInputObject>
    {
        public FileRecordInputType()
        {
            Name = "FileRecordInputType";
            Field(p => p.FilePath).Description("The path of the file.");
            Field<ListGraphType<RecordMetadataInputType>>("metadata",
                description: "Some metadata about the file.",
                resolve: context => context.Source.Metadata);
            Field(p => p.MimeType).Description("The mimetype of the file.");
        }
    }
}
