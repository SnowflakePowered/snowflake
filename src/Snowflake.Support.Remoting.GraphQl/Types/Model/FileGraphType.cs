using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Model.FileSystem;

namespace Snowflake.Support.Remoting.GraphQL.Types.Model
{
    public class FileGraphType : ObjectGraphType<IFile>
    {
        public FileGraphType()
        {
            Name = "File";
            Description = "A manifested file that may or may not.";

            //Field(f => f).Description("The path to the file.");

            Field<StringGraphType>("name",
                description: "The name of the file.",
                resolve: context => context.Source.Name);

            Field<StringGraphType>("path",
                description: "The rooted path to the file, relative to the directory provider.",
                resolve: context => context.Source.RootedPath);

            Field<StringGraphType>("realPath",
                description: "The real path to the file on the file system.",
#pragma warning disable CS0618 // Type or member is obsolete
                resolve: context => context.Source.GetFilePath().FullName);
#pragma warning restore CS0618 // Type or member is obsolete

            Field<GuidGraphType>("guid",
                description: "The unique ID of the file.",
                resolve: context => context.Source.FileGuid);
        }
    }
}
