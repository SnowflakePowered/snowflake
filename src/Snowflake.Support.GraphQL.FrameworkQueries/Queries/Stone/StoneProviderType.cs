﻿using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Stone
{
    public sealed class StoneProviderType
        : ObjectType<IStoneProvider>
    {
        protected override void Configure(IObjectTypeDescriptor<IStoneProvider> descriptor)
        {
            descriptor.Name("StoneQuery")
                .Description("Provides access to Stone platform and controller definitions.");
            descriptor.Field("platforms")
                .Resolve(context => context.Parent<IStoneProvider>().Platforms.Values)
                .Type<NonNullType<ListType<NonNullType<PlatformInfoType>>>>()
                .UseFiltering<PlatformInfoFilter>()
                .Description("Gets the Stone platform definitions matching the search query.");
            descriptor.Field("controllers")
                .Resolve(context => context.Parent<IStoneProvider>().Controllers.Values)
                .Type<NonNullType<ListType<NonNullType<ControllerLayoutType>>>>()
                .UseFiltering<ControllerLayoutFilter>()
                .Description("Gets the Stone controller layout definitions matching the search query.");
        }
    }
}
