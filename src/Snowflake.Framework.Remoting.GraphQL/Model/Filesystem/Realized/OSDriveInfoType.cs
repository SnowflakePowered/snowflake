using HotChocolate.Types;
using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Zio;

namespace Snowflake.Remoting.GraphQL.Model.Filesystem.Contextual
{
    public sealed class OSDriveInfoType
        : ObjectType<DriveInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<DriveInfo> descriptor)
        {
            descriptor.Name("_OSDirectoryInfo__DriveInfo")
               .Description("Describes a directory in the realized, OS-dependent file system.")
               .Implements<OSDirectoryInfoInterface>()
               .Implements<DirectoryInfoInterface>();
            descriptor.Field("lastModifiedTime")
                .Description("The last modified time of the directory, in UTC. This is meaningless for drives.")
                .Type<NonNullType<DateTimeType>>()
                .Resolve(context => DateTime.UtcNow);
            descriptor.Field("createdTime")
                .Description("The creation time of the directory, in UTC. This is meaningless for drives.")
                .Type<NonNullType<DateTimeType>>()
                .Resolve(context => DateTime.UnixEpoch);
            descriptor.Field("osPath")
                .Description("The path of the file on the realized operating system.")
                .Type<NonNullType<OSDirectoryPathType>>()
                .Resolve(context => context.Parent<DriveInfo>().RootDirectory);
            descriptor.Field("path")
                .Description("The path of this directory")
                .Type<NonNullType<OSDirectoryPathType>>()
                .Resolve(context => context.Parent<DriveInfo>().RootDirectory);
            descriptor.Field("name")
                 .Description("The name of the directory.")
                 .Type<NonNullType<StringType>>()
                 .Resolve(context => context.Parent<DriveInfo>().RootDirectory.Name);
            descriptor.Field("isDrive")
                .Description("Whether or not this directory is a drive root.")
                .Type<NonNullType<BooleanType>>()
                .Resolve(true);
            descriptor.Field("isHidden")
                .Description("Whether or not this file is hidden")
                .Type<NonNullType<BooleanType>>()
                .Resolve(context => !context.Parent<DriveInfo>().IsReady);
        }
    }
}
