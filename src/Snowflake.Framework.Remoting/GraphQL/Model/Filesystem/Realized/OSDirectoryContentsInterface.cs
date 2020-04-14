using GraphQL.Types;
using HotChocolate.Types;
using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Filesystem.Contextual
{
    public sealed class OSDirectoryContentsInterface
        : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name("OSDirectoryContents")
                .Description("Describes the contents of a directory in the realized, OS-dependent file system.")
                .Interface<DirectoryContentsInterface>();
            descriptor.Field("root")
                .Description("The root directory.")
                .Type<OSDirectoryInfoInterface>();
            descriptor.Field("files")
               .Description("The files contained in this directory.")
               .Type<NonNullType<ListType<NonNullType<OSFileInfoType>>>>();
            descriptor.Field("directories")
                .Description("The child directories contained in this directory.")
                .Type<NonNullType<ListType<NonNullType<OSDirectoryInfoInterface>>>>();
            descriptor.Field("directoryCount")
                .Description("The number of child directories contained in this directory.")
                .Type<NonNullType<IntType>>();
            descriptor.Field("fileCount")
               .Description("The number of child files contained in this directory.")
               .Type<NonNullType<IntType>>();
        }
    }
}
