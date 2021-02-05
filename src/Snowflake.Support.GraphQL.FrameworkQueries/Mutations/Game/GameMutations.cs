using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Model.Records;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
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
            descriptor.ExtendMutation();
            descriptor.Field("createGame")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Description("Creates a new game in the database.")
                .Argument("input", arg => arg.Type<CreateGameInputType>())
                .Resolve(async ctx =>
                {
                    var gameLibrary = ctx.SnowflakeService<IGameLibrary>();

                    CreateGameInput args = ctx.ArgumentValue<CreateGameInput>("input");
                    var game = await gameLibrary.CreateGameAsync(args.PlatformID);
                    return new GamePayload()
                    {
                        Game = game,
                    };
                })
                .Type<NonNullType<GamePayloadType>>();

            descriptor.Field("deleteGame")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Description("Marks a game as deleted. This does not actually purge the game from the database. " +
                "Instead, a metadata value `game_deleted` is set to true. " +
                "Permanently deleting a game once created from the database is not permitted.")
                .Argument("input", arg => arg.Type<DeleteGameInputType>())
                .Resolve(async ctx =>
                {
                    var gameLibrary = ctx.SnowflakeService<IGameLibrary>();

                    DeleteGameInput args = ctx.ArgumentValue<DeleteGameInput>("input");
                    var game = await gameLibrary.GetGameAsync(args.GameID);
                    game.Record.Metadata["game_deleted"] = "true";
                    await gameLibrary.UpdateGameRecordAsync(game.Record);
                    return new GamePayload()
                    {
                        Game = game,
                    };
                })
                .Type<NonNullType<GamePayloadType>>();
        }
    }
}
