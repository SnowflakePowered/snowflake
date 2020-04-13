using HotChocolate.Types;
using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Records
{
    public sealed class FileType
        : ObjectType<IReadOnlyFile>
    {
        protected override void Configure(IObjectTypeDescriptor<IReadOnlyFile> descriptor)
        {
            descriptor.Name("File")
                .Description("A file with a virtualized path given context, managed by Snowflake by a directory manifest.")
                .BindFieldsExplicitly();

            descriptor.Field(f => f.Name)
                .Description("The file name.");
            descriptor.Field(f => f.Length)
                .Description("The length of the file in bytes. If it does not exist, this is -1.");
            descriptor.Field(f => f.RootedPath)
                .Name("path")
                .Description("The virtual path of the file, relative to the root of the directory context.");
            descriptor.Field("diskPath")
                .Description("The actual path of the file on disk.")
#pragma warning disable CS0618 // Type or member is obsolete
                .Resolver(ctx => ctx.Parent<IFile>().UnsafeGetFilePath().FullName); // lgtm [cs/call-to-obsolete-method]
#pragma warning restore CS0618 // Type or member is obsolete
            descriptor.Field(f => f.FileGuid)
                .Name("fileID")
                .Description("The unique GUID of this file in the manifest.");
        }
    }
}
