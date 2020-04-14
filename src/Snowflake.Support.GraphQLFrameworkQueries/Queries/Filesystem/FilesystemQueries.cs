using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Filesystem;
using Snowflake.Framework.Remoting.GraphQL.Model.Filesystem.Contextual;
using Snowflake.Framework.Remoting.GraphQL.Schema;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.IO;
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
                .Argument("directoryPath", a => a.Type<OSDirectoryPathType>().Description("The path to explore."))
                .Resolver(context => {
                    var path = context.Argument<DirectoryInfo>("directoryPath");
                    if (path == null) return DriveInfo.GetDrives();
                    return path;
                })
                .Type<OSDirectoryContentsInterface>()
                .Description("Provides normalized OS-dependent filesystem access.");
        }
    }

}
