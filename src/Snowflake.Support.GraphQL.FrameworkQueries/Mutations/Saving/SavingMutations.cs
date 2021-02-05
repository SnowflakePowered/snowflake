﻿using HotChocolate;
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
            descriptor.ExtendMutation();
            descriptor.Field("createSaveProfile")
                .Description("Create a new save profile.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", arg => arg.Type<CreateSaveProfileInputType>())
                .Resolve(async ctx =>
                {
                    var gameLibrary = ctx.SnowflakeService<IGameLibrary>();
                    var input = ctx.ArgumentValue<CreateSaveProfileInput>("input");
                    var game = await gameLibrary.GetGameAsync(input.GameID);
                    if (game == null)
                        return ErrorBuilder.New()
                           .SetCode("SAVE_NOTFOUND_GAME")
                           .SetMessage("The specified game does not exist.")
                           .Build();
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
                .Description("Delete a save profile.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", arg => arg.Type<DeleteSaveProfileInputType>())
                .Resolve(async ctx =>
                {
                    var gameLibrary = ctx.SnowflakeService<IGameLibrary>();
                    var input = ctx.ArgumentValue<DeleteSaveProfileInput>("input");
                    var game = await gameLibrary.GetGameAsync(input.GameID);
                    if (game == null)
                        return ErrorBuilder.New()
                           .SetCode("SAVE_NOTFOUND_GAME")
                           .SetMessage("The specified game does not exist.")
                           .Build();
                    var gameSaves = game.WithFiles().WithSaves();
                    var saveProfile = gameSaves.GetProfile(input.ProfileID);
                    if (saveProfile == null)
                        return ErrorBuilder.New()
                           .SetCode("SAVE_NOTFOUND_PROFILE")
                           .SetMessage("The specified save profile does not exist.")
                           .Build();
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
