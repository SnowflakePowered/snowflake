using HotChocolate.Types;
using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Zio;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Filesystem.Contextual
{
    public sealed class OSDirectoryInfoType
        : ObjectType<DirectoryInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<DirectoryInfo> descriptor)
        {
            descriptor.Name("_OSDirectoryInfo__DirectoryInfo")
               .Description("Describes a directory in the realized, OS-dependent file system.")
               .Interface<OSDirectoryInfoInterface>()
               .Interface<DirectoryInfoInterface>();
            descriptor.Field("lastModifiedTime")
                .Description("The last modified time of the directory, in UTC.")
                .Type<NonNullType<DateTimeType>>()
                .Resolver(context => context.Parent<DirectoryInfo>().LastWriteTimeUtc);
            descriptor.Field("createdTime")
                .Description("The creation time of the directory, in UTC.")
                .Type<NonNullType<DateTimeType>>()
                .Resolver(context => context.Parent<DirectoryInfo>().CreationTimeUtc);
            descriptor.Field("osPath")
                .Description("The path of the file on the realized operating system.")
                .Type<NonNullType<OSDirectoryPathType>>()
                .Resolver(context => context.Parent<DirectoryInfo>());
            descriptor.Field("path")
                .Description("The path of this directory")
                .Type<NonNullType<OSDirectoryPathType>>()
                .Resolver(context => context.Parent<DirectoryInfo>());
            descriptor.Field("name")
                 .Description("The name of the directory.")
                 .Type<NonNullType<StringType>>()
                 .Resolver(context => context.Parent<DirectoryInfo>().Name);
            descriptor.Field("isDrive")
                .Description("Whether or not this directory is a drive root.")
                .Type<NonNullType<BooleanType>>()
                .Resolver(context => context.Parent<DirectoryInfo>().Parent == null);
            descriptor.Field("isHidden")
                .Description("Whether or not this directory is hidden")
                .Type<NonNullType<BooleanType>>()
                .Resolver(context => context.Parent<DirectoryInfo>().Attributes.HasFlag(FileAttributes.Hidden));
        }
    }
}
