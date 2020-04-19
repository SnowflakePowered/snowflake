using HotChocolate.Types;
using Snowflake.Filesystem;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zio;

namespace Snowflake.Remoting.GraphQL.Model.Filesystem.Contextual
{
    public sealed class ContextualFileInfoType
        : ObjectType<IReadOnlyFile>
    {
        protected override void Configure(IObjectTypeDescriptor<IReadOnlyFile> descriptor)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            descriptor.Name("ContextualFileInfo")
                .Description("Describes a file in a contextual filesystem.")
                .Interface<FileInfoInterface>();
            descriptor.Field("extension")
                .Description("The extension of the file.")
                .Type<NonNullType<StringType>>()
                .Resolver(context => Path.GetExtension(context.Parent<IReadOnlyFile>().Name));
            descriptor.Field("name")
                .Description("The name of the file.")
                .Type<NonNullType<StringType>>()
                .Resolver(context => context.Parent<IReadOnlyFile>().Name);
            descriptor.Field("osPath")
                .Description("The path of the file on the realized operating system.")
                .Type<NonNullType<OSFilePathType>>()
                .Resolver(context => context.Parent<IReadOnlyFile>().UnsafeGetFilePath());
            descriptor.Field("lastModifiedTime")
                .Description("The last modified time of the file, in UTC.")
                .Type<NonNullType<DateTimeType>>()
                .Resolver(context => context.Parent<IReadOnlyFile>().UnsafeGetFilePath().LastWriteTimeUtc);
            descriptor.Field("createdTime")
                .Description("The creation time of the file, in UTC.")
                .Type<NonNullType<DateTimeType>>()
                .Resolver(context => context.Parent<IReadOnlyFile>().UnsafeGetFilePath().CreationTimeUtc);
            descriptor.Field("size")
                .Description("The size of the file, in bytes.")
                .Type<NonNullType<IntType>>()
                .Resolver(context => context.Parent<IReadOnlyFile>().UnsafeGetFilePath().Length);
            descriptor.Field("isLink")
                .Description("Whether or not this file is a link to a file outside the directory context.")
                .Type<NonNullType<BooleanType>>()
                .Resolver(context => context.Parent<IReadOnlyFile>().IsLink);
            descriptor.Field("path")
                .Description("The contextual path to this file.")
                .Type<NonNullType<FilePathType>>()
                .Resolver(context => (UPath)context.Parent<IReadOnlyFile>().RootedPath);
            descriptor.Field("fileId")
               .Description("The manifest GUID of this file.")
               .Type<NonNullType<UuidType>>()
               .Resolver(context => context.Parent<IReadOnlyFile>().FileGuid);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
