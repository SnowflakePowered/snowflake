using HotChocolate.Types;
using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Filesystem.Contextual
{
    public sealed class OSDriveContentsType
        : ObjectType<DriveInfo[]>
    {
        protected override void Configure(IObjectTypeDescriptor<DriveInfo[]> descriptor)
        {
            descriptor.Name("__OSDirectoryContents__DriveInfo")
                .Description("Describes the contents of a directory in the realized, OS-dependent file system.")
                .Interface<OSDirectoryContentsInterface>()
                .Interface<DirectoryContentsInterface>();
            descriptor.Field("root")
                .Description("The root directory.")
                .Type<OSDirectoryInfoType>()
                .Resolver(context => null);
            descriptor.Field("files")
               .Description("The files contained in this directory.")
               .Type<NonNullType<ListType<NonNullType<OSFileInfoType>>>>()
               .Resolver(context => Enumerable.Empty<FileInfo>());
            descriptor.Field("directories")
                .Description("The child directories contained in this directory.")
                .Type<NonNullType<ListType<NonNullType<OSDriveInfoType>>>>()
                .Resolver(context => context.Parent<DriveInfo[]>());
            descriptor.Field("directoryCount")
                .Description("The number of child directories contained in this directory.")
                .Type<NonNullType<IntType>>()
                .Resolver(context => context.Parent<DriveInfo[]>().Length);
            descriptor.Field("fileCount")
               .Description("The number of child files contained in this directory.")
               .Type<NonNullType<IntType>>().Resolver(0);
        }
    }
}
