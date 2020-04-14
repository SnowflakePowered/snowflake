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
    public sealed class OSDriveInfoType
        : ObjectType<DriveInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<DriveInfo> descriptor)
        {
            descriptor.Name("__OSDirectoryInfo__DriveInfo")
               .Description("Describes a directory in the realized, OS-dependent file system.")
               .Interface<OSDirectoryInfoInterface>()
               .Interface<DirectoryInfoInterface>();
            descriptor.Field("lastModifiedTime")
                .Description("The last modified time of the directory, in UTC. This is meaningless for drives.")
                .Type<DateTimeType>()
                .Resolver(context => DateTime.UtcNow);
            descriptor.Field("createdTime")
                .Description("The creation time of the directory, in UTC. This is meaningless for drives.")
                .Type<DateTimeType>()
                .Resolver(context => DateTime.UnixEpoch);
            descriptor.Field("osPath")
                .Description("The path of the file on the realized operating system.")
                .Type<OSDirectoryPathType>()
                .Resolver(context => context.Parent<DriveInfo>().RootDirectory);
            descriptor.Field("path")
                .Description("The path of this directory")
                .Type<OSDirectoryPathType>()
                .Resolver(context => context.Parent<DriveInfo>().RootDirectory);
            descriptor.Field("name")
                 .Description("The name of the directory.")
                 .Type<StringType>()
                 .Resolver(context => context.Parent<DriveInfo>().RootDirectory.Name);
            descriptor.Field("isDrive")
                .Description("Whether or not this directory is a drive root.")
                .Type<BooleanType>()
                .Resolver(true);
            descriptor.Field("isHidden")
                .Description("Whether or not this file is hidden")
                .Type<BooleanType>()
                .Resolver(context => !context.Parent<DriveInfo>().IsReady);
        }
    }
}
