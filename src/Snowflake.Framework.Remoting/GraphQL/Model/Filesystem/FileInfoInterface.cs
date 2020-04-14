using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Filesystem
{
    public sealed class FileInfoInterface
        : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name("FileInfo")
                .Description("Describes a file in a filesystem, regardless of whether the filesystem is virtualized or realized (OS).");
            descriptor.Field("extension")
                .Description("The extension of the file.")
                .Type<NonNullType<StringType>>();
            descriptor.Field("name")
                .Description("The name of the file.")
                .Type<NonNullType<StringType>>();
            descriptor.Field("osPath")
                .Description("The path of the file on the realized operating system.")
                .Type<NonNullType<OSFilePathType>>();
            descriptor.Field("lastModifiedTime")
                .Description("The last modified time of the file, in UTC.")
                .Type<NonNullType<DateTimeType>>();
            descriptor.Field("createdTime")
                .Description("The creation time of the file, in UTC.")
                .Type<NonNullType<DateTimeType>>();
            descriptor.Field("size")
                .Description("The size of the file, in bytes.")
                .Type<NonNullType<IntType>>();
        }
    }
}
