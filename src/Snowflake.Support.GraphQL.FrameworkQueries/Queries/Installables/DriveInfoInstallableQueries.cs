using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Installation;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Installation.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Remoting.GraphQL;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Installables
{
    public sealed class DriveInfoInstallableQueries
        : ObjectTypeExtension<DriveInfo[]>
    {
        protected override void Configure(IObjectTypeDescriptor<DriveInfo[]> descriptor)
        {
            descriptor
                .Name("_OSDirectoryContents__DriveInfo");
            descriptor.Field("installables")
                .Type<NonNullType<ListType<NonNullType<InstallableType>>>>()
                .Argument("platformId", arg => arg.Type<NonNullType<PlatformIdType>>()
                    .Description("The platform to look up installables for."))
                .Resolver(ctx =>
                {
                    var installers = ctx.SnowflakeService<IPluginManager>().GetCollection<IGameInstaller>();
                    var platform = ctx.Argument<PlatformId>("platformId");
                    var drives = ctx.Parent<DriveInfo[]>();

                    return from installer in installers
                           from driveRoot in drives.Select(d => d.RootDirectory)
                           from installable in installer.GetInstallables(platform, new[] { driveRoot })
                           select installable;
                });
        }
    }
}
