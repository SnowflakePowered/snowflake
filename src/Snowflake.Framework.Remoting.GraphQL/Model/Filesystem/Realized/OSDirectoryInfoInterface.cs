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
    public sealed class OSDirectoryInfoInterface
        : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name("OSDirectoryInfo")
               .Description("Describes a directory in the realized, OS-dependent file system.")
               .Implements<DirectoryInfoInterface>();
            descriptor.Field("lastModifiedTime")
                .Description("The last modified time of the directory, in UTC.")
                .Type<NonNullType<DateTimeType>>();
            descriptor.Field("createdTime")
                .Description("The creation time of the directory, in UTC.")
                .Type<NonNullType<DateTimeType>>();
            descriptor.Field("osPath")
                .Description("The path of the file on the realized operating system.")
                .Type<NonNullType<OSDirectoryPathType>>();
            descriptor.Field("path")
                .Description("The path of this directory")
                .Type<NonNullType<OSDirectoryPathType>>();
            descriptor.Field("name")
                 .Description("The name of the directory.")
                 .Type<NonNullType<StringType>>();
            descriptor.Field("isDrive")
                 .Description("Whether or not this directory is a drive root.")
                 .Type<NonNullType<BooleanType>>();
            descriptor.Field("isHidden")
                .Description("Whether nor not this directory is hidden.")
                .Type<NonNullType<BooleanType>>();
        }
    }
}
