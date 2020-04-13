using GraphQL.Types;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Filesystem
{
    public sealed class DirectoryInfoInterface
        : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name("DirectoryInfo")
                .Description("Describes a directory in a filesystem, regardless of whether the filesystem is virtualized or realized (OS).");
            descriptor.Field("lastModifiedTime")
                .Description("The last modified time of the directory, in UTC.")
                .Type<DateTimeType>();
            descriptor.Field("createdTime")
                .Description("The creation time of the directory, in UTC.")
                .Type<DateTimeType>();
            descriptor.Field("name")
                .Description("The name of the directory.")
                .Type<StringType>();
            descriptor.Field("osPath")
                .Description("The path of the file on the realized operating system.")
                .Type<OSDirectoryPathType>();
            descriptor.Field("cursor")
                .Description("The name of the field that acts as a unique cursor to the directoryPath argument of the fs API that produced this DirectoryInfo.")
                .Type<StringType>();
        }
    }
}
