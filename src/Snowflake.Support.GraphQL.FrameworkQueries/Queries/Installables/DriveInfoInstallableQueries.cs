﻿using HotChocolate.Types;
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
    public sealed class DriveInfoInstallableQueries
        : ObjectTypeExtension<DriveInfo[]>
    {
        protected override void Configure(IObjectTypeDescriptor<DriveInfo[]> descriptor)
        {
            descriptor
                .Name("__OSDirectoryContents__DriveInfo");
            descriptor.Field("installables")
                .Type<NonNullType<ListType<NonNullType<InstallableType>>>>()
                .Argument("platformID", arg => arg.Type<NonNullType<PlatformIdType>>()
                    .Description("The platform to look up installables for."))
                .Resolver(ctx =>
                {
                    var installers = ctx.Service<IPluginManager>().GetCollection<IGameInstaller>();
                    var platform = ctx.Argument<PlatformId>("platformID");
                    var drives = ctx.Parent<DriveInfo[]>();

                    return from installer in installers
                           from driveRoot in drives.Select(d => d.RootDirectory)
                           from installable in installer.GetInstallables(platform, new[] { driveRoot })
                           select installable;
                });
        }
    }
}