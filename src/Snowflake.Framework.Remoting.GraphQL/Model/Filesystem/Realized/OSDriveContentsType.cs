using HotChocolate.Types;
using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Filesystem.Contextual
{
    public sealed class OSDriveContentsType
        : ObjectType<DriveInfo[]>
    {
        protected override void Configure(IObjectTypeDescriptor<DriveInfo[]> descriptor)
        {
            descriptor.Name("_OSDirectoryContents__DriveInfo")
                .Description("Describes the contents of a directory in the realized, OS-dependent file system.")
                .Implements<OSDirectoryContentsInterface>()
                .Implements<DirectoryContentsInterface>();
            descriptor.Field("root")
                .Description("The root directory.")
                .Type<OSDirectoryInfoType>()
                .Resolve(context => null);
            descriptor.Field("files")
               .Description("The files contained in this directory.")
               .Type<NonNullType<ListType<NonNullType<OSFileInfoType>>>>()
               .Resolve(context => Enumerable.Empty<FileInfo>());
            descriptor.Field("directories")
                .Description("The child directories contained in this directory.")
                .Type<NonNullType<ListType<NonNullType<OSDriveInfoType>>>>()
                .Resolve(context => context.Parent<DriveInfo[]>());
            descriptor.Field("directoryCount")
                .Description("The number of child directories contained in this directory.")
                .Type<NonNullType<IntType>>()
                .Resolve(context => context.Parent<DriveInfo[]>().Length);
            descriptor.Field("fileCount")
               .Description("The number of child files contained in this directory.")
               .Type<NonNullType<IntType>>().Resolve(0);
        }
    }
}
