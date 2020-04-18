using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Installation;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Installation.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Installables
{
    public sealed class DirectoryInfoInstallableQueries
        : ObjectTypeExtension<DirectoryInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<DirectoryInfo> descriptor)
        {
            descriptor
                .Name("_OSDirectoryContents__DirectoryInfo");
            descriptor.Field("installables")
                .Type<NonNullType<ListType<NonNullType<InstallableType>>>>()
                .Argument("platformId", arg => arg.Type<NonNullType<PlatformIdType>>()
                    .Description("The platform to look up installables for."))
                .Resolver(ctx =>
                {
                    var installers = ctx.Service<IPluginManager>().GetCollection<IGameInstaller>();
                    var platform = ctx.Argument<PlatformId>("platformId");
                    var dirs = ctx.Parent<DirectoryInfo>();

                    return from installer in installers
                           from installable in installer.GetInstallables(platform, dirs.EnumerateFileSystemInfos())
                           select installable;
                });
        }
    }
}
