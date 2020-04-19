using HotChocolate.Types;
using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Filesystem.Contextual
{
    public sealed class OSDirectoryContentsType
        : ObjectType<DirectoryInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<DirectoryInfo> descriptor)
        {
            descriptor.Name("_OSDirectoryContents__DirectoryInfo")
                .Description("Describes the contents of a directory in the realized, OS-dependent file system.")
                .Interface<OSDirectoryContentsInterface>()
                .Interface<DirectoryContentsInterface>();
            descriptor.Field("root")
                .Description("The root directory.")
                .Type<OSDirectoryInfoType>()
                .Resolver(context => context.Parent<DirectoryInfo>());
            descriptor.Field("files")
               .Description("The files contained in this directory.")
               .Type<NonNullType<ListType<NonNullType<OSFileInfoType>>>>()
               .Resolver(context => context.Parent<DirectoryInfo>().EnumerateFiles());
            descriptor.Field("directories")
                .Description("The child directories contained in this directory.")
                .Type<NonNullType<ListType<NonNullType<OSDirectoryInfoType>>>>()
                .Resolver(context => context.Parent<DirectoryInfo>().EnumerateDirectories());
            descriptor.Field("directoryCount")
                .Description("The number of child directories contained in this directory.")
                .Type<NonNullType<IntType>>()
                .Resolver(context => context.Parent<DirectoryInfo>().EnumerateDirectories().LongCount());
            descriptor.Field("fileCount")
               .Description("The number of child files contained in this directory.")
               .Type<NonNullType<IntType>>().Resolver(context => context.Parent<DirectoryInfo>().EnumerateDirectories().LongCount());
        }
    }
}
