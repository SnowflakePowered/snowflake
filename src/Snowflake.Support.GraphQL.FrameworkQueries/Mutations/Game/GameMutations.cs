using HotChocolate.Types;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Game
{
    public sealed class GameMutations
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Mutation");
            descriptor.Field("createGame")
                .Argument("input", arg => arg.Type<CreateGameInputType>())
                .Resolver(async ctx =>
                {
                    var gameLibrary = ctx.Service<IGameLibrary>();
                    CreateGameInput args = ctx.Argument<CreateGameInput>("input");
                    var game = await gameLibrary.CreateGameAsync(args.PlatformID);
                    return new GamePayload()
                    {
                        Game = game,
                        ClientMutationID = args.ClientMutationID,
                    };
                })
                .Type<GamePayloadType>();
        }
    }
}
