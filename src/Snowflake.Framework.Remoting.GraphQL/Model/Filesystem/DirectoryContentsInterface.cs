using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Filesystem
{
    public sealed class DirectoryContentsInterface
        : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name("DirectoryContents")
                .Description("Describes the contents of a directory as an edge.");
            descriptor.Field("root")
                .Description("The root directory.")
                .Type<DirectoryInfoInterface>();
            descriptor.Field("files")
               .Description("The files contained in this directory.")
               .Type<NonNullType<ListType<NonNullType<FileInfoInterface>>>>();
            descriptor.Field("directories")
                .Description("The child directories contained in this directory.")
                .Type<NonNullType<ListType<NonNullType<DirectoryInfoInterface>>>>();
            descriptor.Field("directoryCount")
                .Description("The number of child directories contained in this directory.")
                .Type<NonNullType<IntType>>();
            descriptor.Field("fileCount")
               .Description("The number of child files contained in this directory.")
               .Type<NonNullType<IntType>>();
        }
    }
}
