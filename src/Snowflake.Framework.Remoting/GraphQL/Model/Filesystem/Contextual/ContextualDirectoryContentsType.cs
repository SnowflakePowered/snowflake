using HotChocolate.Types;
using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Filesystem.Contextual
{
    public sealed class ContextualDirectoryContentsType
        : ObjectType<IReadOnlyDirectory>
    {
        protected override void Configure(IObjectTypeDescriptor<IReadOnlyDirectory> descriptor)
        {
            descriptor.Name("ContextualDirectoryContents")
                .Description("Describes the contents of a directory on a contextual virtual filesystem.")
                .Interface<DirectoryContentsInterface>();
            descriptor.Field("root")
                .Description("The root directory.")
                .Type<ContextualDirectoryInfoType>()
                .Resolver(context => context.Parent<IReadOnlyDirectory>());
            descriptor.Field("files")
               .Description("The files contained in this directory.")
               .Type<NonNullType<ListType<NonNullType<ContextualFileInfoType>>>>()
               .Resolver(context => context.Parent<IReadOnlyDirectory>().EnumerateFiles());
            descriptor.Field("directories")
                .Description("The child directories contained in this directory.")
                .Type<NonNullType<ListType<NonNullType<ContextualDirectoryInfoType>>>>()
                .Resolver(context => context.Parent<IReadOnlyDirectory>().EnumerateDirectories());
            descriptor.Field("directoryCount")
                .Description("The number of child directories contained in this directory.")
                .Type<NonNullType<IntType>>()
                .Resolver(context => context.Parent<IReadOnlyDirectory>().EnumerateDirectories().Count());
            descriptor.Field("fileCount")
               .Description("The number of child files contained in this directory.")
               .Type<NonNullType<IntType>>().Resolver(context => context.Parent<IReadOnlyDirectory>().EnumerateFiles().Count());
        }
    }
}
