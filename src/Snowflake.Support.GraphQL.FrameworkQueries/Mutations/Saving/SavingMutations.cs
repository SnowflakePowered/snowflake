using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Orchestration.Saving;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Saving
{
    public sealed class SavingMutations
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Mutation");
            descriptor.Field("createSaveProfile")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", arg => arg.Type<CreateSaveProfileInputType>())
                .Resolver(async ctx =>
                {
                    var gameLibrary = ctx.SnowflakeService<IGameLibrary>();
                    var input = ctx.Argument<CreateSaveProfileInput>("input");
                    var game = await gameLibrary.GetGameAsync(input.GameID);
                    if (game == null) 
                        throw new ArgumentException("The specified game was not found.");
                    var saveProfile = game.WithFiles().WithSaves()
                        .CreateProfile(input.ProfileName, input.SaveType, input.ManagementStrategy);
                    return new CreateSaveProfilePayload()
                    {
                        SaveProfile = saveProfile,
                        Game = game,
                    };
                })
                .Type<NonNullType<CreateSaveProfilePayloadType>>();

            descriptor.Field("deleteSaveProfile")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", arg => arg.Type<DeleteSaveProfileInputType>())
                .Resolver(async ctx =>
                {
                    var gameLibrary = ctx.SnowflakeService<IGameLibrary>();
                    var input = ctx.Argument<DeleteSaveProfileInput>("input");
                    var game = await gameLibrary.GetGameAsync(input.GameID);
                    if (game == null)
                        throw new ArgumentException("The specified game was not found.");
                    var gameSaves = game.WithFiles().WithSaves();
                    var saveProfile = gameSaves.GetProfile(input.ProfileID);
                    if (saveProfile == null)
                        throw new ArgumentException("The specified save profile was not found.");
                    gameSaves.DeleteProfile(input.ProfileID);
                    return new DeleteSaveProfilePayload()
                    {
                        SaveProfile = saveProfile,
                        Game = game,
                    };
                })
                .Type<NonNullType<DeleteSaveProfilePayloadType>>();
        }
    }
}
