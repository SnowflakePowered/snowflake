using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Model.Records;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Game
{
    public sealed class GameMetadataMutations
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Mutation");
            
            descriptor.Field("updateGameMetadata")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Description("Update or creates the given metadata value for a game.")
                .Argument("input", arg => arg.Type<UpdateGameMetadataInputType>())
                .Resolver(async ctx =>
                {
                    var gameLibrary = ctx.Service<IGameLibrary>();
                    UpdateGameMetadataInput args = ctx.Argument<UpdateGameMetadataInput>("input");

                    var game = await gameLibrary.GetGameAsync(args.GameID);
                    game.Record.Metadata[args.MetadataKey] = args.MetadataValue;
                    await gameLibrary.UpdateGameRecordAsync(game.Record);
                    return new UpdateGameMetadataPayload()
                    {
                        Game = game,
                        Metadata = (game.Record.Metadata as IDictionary<string, IRecordMetadata>)[args.MetadataKey],
                    };
                })
                .Type<NonNullType<UpdateGameMetadataPayloadType>>();

            descriptor.Field("deleteGameMetadata")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Description("Removes a metadata entry for a game.")
                .Argument("input", arg => arg.Type<DeleteGameMetadataInputType>())
                .Resolver(async ctx =>
                {
                    var gameLibrary = ctx.Service<IGameLibrary>();

                    DeleteGameMetadataInput args = ctx.Argument<DeleteGameMetadataInput>("input");

                    var game = await gameLibrary.GetGameAsync(args.GameID);
                    (game.Record.Metadata as IDictionary<string, IRecordMetadata>)
                        .TryGetValue(args.MetadataKey, out IRecordMetadata metadata);
                    if (metadata != null)
                    {
                        game.Record.Metadata[metadata.Key] = null;
                        await gameLibrary.UpdateGameRecordAsync(game.Record);
                    }
                    return new DeleteGameMetadataPayload()
                    {
                        Game = game,
                        Metadata = metadata,
                    };
                })
                .Type<NonNullType<DeleteGameMetadataPayloadType>>();
        }
    }
}
