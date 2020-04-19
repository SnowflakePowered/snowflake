using HotChocolate.Types;
using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zio;

namespace Snowflake.Remoting.GraphQL.Model.Filesystem.Contextual
{
    public sealed class ContextualDirectoryInfoType
        : ObjectType<IReadOnlyDirectory>
    {
#pragma warning disable CS0618 // Type or member is obsolete
        protected override void Configure(IObjectTypeDescriptor<IReadOnlyDirectory> descriptor)
        {
            descriptor.Name("ContextualDirectoryInfo")
               .Description("Describes a directory in a contextual virtualized filesystem.")
               .Interface<DirectoryInfoInterface>();
            descriptor.Field("lastModifiedTime")
                .Description("The last modified time of the directory, in UTC.")
                .Type<NonNullType<DateTimeType>>()
                .Resolver(context => context.Parent<IReadOnlyDirectory>().UnsafeGetPath().LastWriteTimeUtc);
            descriptor.Field("createdTime")
                .Description("The creation time of the directory, in UTC.")
                .Type<NonNullType<DateTimeType>>()
                .Resolver(context => context.Parent<IReadOnlyDirectory>().UnsafeGetPath().CreationTimeUtc);
            descriptor.Field("osPath")
                .Description("The path of the file on the realized operating system.")
                .Type<NonNullType<OSDirectoryPathType>>()
                .Resolver(context => context.Parent<IReadOnlyDirectory>().UnsafeGetPath());
            descriptor.Field("path")
                .Description("The path of this directory relative to the context of the virtualized filesystem.")
                .Type<NonNullType<DirectoryPathType>>()
                .Resolver(context => (UPath)context.Parent<IReadOnlyDirectory>().RootedPath);
            descriptor.Field("name")
                  .Description("The name of the directory.")
                  .Type<NonNullType<StringType>>()
                  .Resolver(context => context.Parent<IReadOnlyDirectory>().Name);
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
