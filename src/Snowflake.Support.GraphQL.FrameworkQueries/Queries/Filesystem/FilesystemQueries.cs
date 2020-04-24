using HotChocolate.Types;
using Microsoft.DotNet.PlatformAbstractions;
using Snowflake.Remoting.GraphQL.Model.Filesystem;
using Snowflake.Remoting.GraphQL.Model.Filesystem.Contextual;
using Snowflake.Remoting.GraphQL.Schema;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Filesystem
{
    public class FilesystemQueries
       : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Query");
            descriptor.Field("filesystem")
                .Argument("directoryPath",
                    a => a.Type<OSDirectoryPathType>()
                    .Description("The path to explore. If this is null, returns a listing of drives on Windows, " +
                    "or the root directory on a Unix-like system."))
                .Resolver(context => {
                    var path = context.Argument<DirectoryInfo>("directoryPath");
                    if ((path == null) && RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return DriveInfo.GetDrives();
                    if ((path == null) &&
                        (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                        || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)) return new DirectoryInfo("/");

                    if (path?.Exists ?? false) return path;
                    return null;
                })
                .Type<OSDirectoryContentsInterface>()
                .Description("Provides normalized OS-dependent filesystem access." +
                "Returns null if the specified path does not exist.");
        }
    }
}
