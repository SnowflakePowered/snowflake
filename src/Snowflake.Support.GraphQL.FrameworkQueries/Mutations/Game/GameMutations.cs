using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Model.Records;
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
                .Description("Creates a new game in the database.")
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

            descriptor.Field("deleteGame")
                .Description("Marks a game as deleted. This does not actually purge the game from the database. " +
                "Instead, a metadata value `game_deleted` is set to true. " +
                "Permanently deleting a game once created from the database is not permitted.")
                .Argument("input", arg => arg.Type<DeleteGameInputType>())
                .Resolver(async ctx =>
                {
                    var gameLibrary = ctx.Service<IGameLibrary>();
                    DeleteGameInput args = ctx.Argument<DeleteGameInput>("input");
                    var game = await gameLibrary.GetGameAsync(args.GameID);
                    game.Record.Metadata["game_deleted"] = "true";
                    await gameLibrary.UpdateGameRecordAsync(game.Record);
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
