﻿using HotChocolate.Types;
using Snowflake.Remoting.GraphQL;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Stone
{
    public class StoneQueries
       : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.ExtendQuery();
            descriptor.Field("stone")
                .Description("Provides access to Stone platform and controller definitions.")
                .Type<NonNullType<StoneProviderType>>()
                .Resolve(ctx => ctx.SnowflakeService<IStoneProvider>());
        }
    }
}
