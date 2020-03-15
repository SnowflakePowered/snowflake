﻿using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Game
{
    public sealed class GameNodeQueries
         : ObjectTypeExtension<IGame>
    {
        protected override void Configure(IObjectTypeDescriptor<IGame> descriptor)
        {
            descriptor.Name("Game");
            descriptor.Interface<NodeType>();
            descriptor.Field("id")
                .Type<IdType>()
                .Resolver(ctx => ctx.Parent<IGame>().Record.RecordID);

            descriptor.AsNode()
                .NodeResolver<Guid>(async (ctx, id) => await ctx.Service<IGameLibrary>().GetGameAsync(id));
        }
    }
}
