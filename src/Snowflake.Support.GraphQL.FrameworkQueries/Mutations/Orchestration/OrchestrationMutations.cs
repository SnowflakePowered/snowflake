using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Saving;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Services;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration
{
    public sealed class OrchestrationMutations
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Mutation");
            descriptor.Field("createEmulationInstance")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Argument("input", a => a.Type<CreateEmulationInstanceInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<CreateEmulationInstanceInput>("input");
                    var orchestrator = ctx.SnowflakeService<IPluginManager>()
                        .GetCollection<IEmulatorOrchestrator>()[input.Orchestrator];
                    if (orchestrator == null) throw new ArgumentException("The specified orchestrator was not found.");
                    var game = await ctx.SnowflakeService<IGameLibrary>()
                        .GetGameAsync(input.GameID);
                    if (game == null)
                        throw new ArgumentException("The specified game was not found.");
                    var compatibility = orchestrator.CheckCompatibility(game);
                    if (compatibility != EmulatorCompatibility.Ready)
                    {
                        switch (compatibility)
                        {
                            case EmulatorCompatibility.MissingSystemFiles:
                                return ErrorBuilder.New()
                                    .SetCode("ORCH_GAME_MISSING_SYSTEM_FILES")
                                    .SetMessage("The specified game can not be run. The emulator requires system files that could not be found.");
                            case EmulatorCompatibility.RequiresValidation:
                                return ErrorBuilder.New()
                                    .SetCode("ORCH_GAME_REQUIRES_VALIDATION")
                                    .SetMessage("The specified game must be validated before it can run.");
                            case EmulatorCompatibility.Unsupported:
                                return ErrorBuilder.New()
                                    .SetCode("ORCH_GAME_UNSUPPORTED")
                                    .SetMessage("The specified game is unsupported by this orchestrator.");
                        }
                    }
                    var ports = ctx.SnowflakeService<IEmulatedPortsManager>();
                    if (!ctx.SnowflakeService<IStoneProvider>().Platforms.TryGetValue(game.Record.PlatformID, out var platform))
                    {
                        throw new ArgumentException("The specified game has an invalid platform ID.");
                    }
                    var controllers = (from i in Enumerable.Range(0, platform.MaximumInputs)
                                       select ports.GetControllerAtPort(orchestrator, platform.PlatformID, i)).ToList();
                    var save = game.WithFiles().WithSaves().GetProfile(input.SaveProfileID);
                    if (save == null) throw new ArgumentException("The specified save profile could not be found.");
                    var instance = orchestrator.ProvisionEmulationInstance(game, controllers, input.CollectionID, save);

                    var guid = Guid.NewGuid();
                    if (ctx.GetGameCache().TryAdd(guid, instance))
                        return new EmulationInstancePayload()
                        {
                            GameEmulation = instance,
                            InstanceID = guid
                        };
                    throw new InvalidOperationException("Could not create emulation instance.");
                }).Type<NonNullType<EmulationInstancePayloadType>>();
            descriptor.Field("cleanupEmulation")
                .Description("Immediately shuts down and cleans up the game emulation. This may or may not persist the " +
                "save game depending on the emulator, but there may be data loss if the game is not saved properly.")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Argument("input", arg => arg.Type<EmulationInstanceInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<EmulationInstanceInput>("input");
                    if (!ctx.GetGameCache().TryGetValue(input.InstanceID, out var gameEmulation))
                    {
                        return ErrorBuilder.New()
                            .SetCode("ORCH_INSTANCE_NOT_FOUND")
                            .SetMessage("The specified orchestration instance was not found.");
                    }
                    bool success = false;
                    try
                    {
                        await gameEmulation.DisposeAsync();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        return ErrorBuilder.New()
                            .SetException(e)
                            .SetCode("ORCH_ERR_CLEANUPEMULATION")
                            .SetMessage("A fatal error occurred with setting up the environment for this game emulation.");
                    }
                    finally
                    {
                        if (success)
                        {
                            ctx.GetGameCache().TryRemove(input.InstanceID, out var _);
                        }
                    }
                    return new CleanupEmulationPayload()
                    {
                        Success = success,
                        InstanceID = input.InstanceID
                    };
                })
                .Type<NonNullType<CleanupEmulationPayloadType>>();
            descriptor.Field("setupEmulationEnvironment")
                .Description("Prepares the emulation environment for this game emulation.")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Argument("input", arg => arg.Type<EmulationInstanceInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<EmulationInstanceInput>("input");
                    if (!ctx.GetGameCache().TryGetValue(input.InstanceID, out var gameEmulation))
                    {
                        return ErrorBuilder.New()
                            .SetCode("ORCH_INSTANCE_NOT_FOUND")
                            .SetMessage("The specified orchestration instance was not found.");
                    }
                    if (gameEmulation.EmulationState != GameEmulationState.RequiresSetupEnvironment)
                        return ErrorBuilder.New()
                            .SetCode("ORCH_INVALID_STATE")
                            .SetMessage("The specified orchestration mutation is not valid for the current state of the game emulation.");
                    try
                    {
                        await gameEmulation.SetupEnvironment();
                    }
                    catch (Exception e)
                    {
                        return ErrorBuilder.New()
                            .SetException(e)
                            .SetCode("ORCH_ERR_SETUPEMULATIONENVIRONMENT")
                            .SetMessage("A fatal error occurred with setting up the environment for this game emulation.");
                    }
                    return new EmulationInstancePayload()
                    {
                        GameEmulation = gameEmulation,
                        InstanceID = input.InstanceID,
                    };
                })
                .Type<NonNullType<EmulationInstancePayloadType>>();
            descriptor.Field("compileEmulationConfiguration")
               .Description("Compiles the configuation file for this emulator.")
               .UseAutoSubscription()
               .UseClientMutationId()
               .Argument("input", arg => arg.Type<EmulationInstanceInputType>())
               .Resolver(async ctx =>
               {
                   var input = ctx.Argument<EmulationInstanceInput>("input");
                   if (!ctx.GetGameCache().TryGetValue(input.InstanceID, out var gameEmulation))
                   {
                       return ErrorBuilder.New()
                           .SetCode("ORCH_INSTANCE_NOT_FOUND")
                           .SetMessage("The specified orchestration instance was not found.");
                   }
                   if (gameEmulation.EmulationState != GameEmulationState.RequiresCompileConfiguration)
                       return ErrorBuilder.New()
                           .SetCode("ORCH_INVALID_STATE")
                           .SetMessage("The specified orchestration mutation is not valid for the current state of the game emulation.");
                   try
                   {
                       await gameEmulation.CompileConfiguration();
                   }
                   catch (Exception e)
                   {
                       return ErrorBuilder.New()
                           .SetException(e)
                           .SetCode("ORCH_ERR_COMPILEEMULATIONCONFIGURATION")
                           .SetMessage("A fatal error occurred with compiling the configuration for this game emulation.");
                   }
                   return new EmulationInstancePayload()
                   {
                       GameEmulation = gameEmulation,
                       InstanceID = input.InstanceID,
                   };
               })
               .Type<NonNullType<EmulationInstancePayloadType>>();
            descriptor.Field("restoreEmulationSave")
                .Description("Restores the save game from the save profile of the game emulation into the emulation working folder.")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Argument("input", arg => arg.Type<EmulationInstanceInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<EmulationInstanceInput>("input");
                    if (!ctx.GetGameCache().TryGetValue(input.InstanceID, out var gameEmulation))
                    {
                        return ErrorBuilder.New()
                            .SetCode("ORCH_INSTANCE_NOT_FOUND")
                            .SetMessage("The specified orchestration instance was not found.");
                    }
                    if (gameEmulation.EmulationState != GameEmulationState.RequiresRestoreSaveGame)
                        return ErrorBuilder.New()
                            .SetCode("ORCH_INVALID_STATE")
                            .SetMessage("The specified orchestration mutation is not valid for the current state of the game emulation.");
                    try
                    {
                        await gameEmulation.RestoreSaveGame();
                    }
                    catch (Exception e)
                    {
                        return ErrorBuilder.New()
                            .SetException(e)
                            .SetCode("ORCH_ERR_RESTOREEMULATIONSAVE")
                            .SetMessage("An error occurred with restoring the save profile for this game emulation.");
                    }
                    return new EmulationInstancePayload()
                    {
                        GameEmulation = gameEmulation,
                        InstanceID = input.InstanceID,
                    };
                })
                .Type<NonNullType<EmulationInstancePayloadType>>();
            descriptor.Field("startEmulation")
                .Description("Starts the specified emulation")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Argument("input", arg => arg.Type<EmulationInstanceInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<EmulationInstanceInput>("input");
                    if (!ctx.GetGameCache().TryGetValue(input.InstanceID, out var gameEmulation))
                    {
                        return ErrorBuilder.New()
                            .SetCode("ORCH_INSTANCE_NOT_FOUND")
                            .SetMessage("The specified orchestration instance was not found.");
                    }
                    if (gameEmulation.EmulationState != GameEmulationState.CanStartEmulation)
                        return ErrorBuilder.New()
                            .SetCode("ORCH_INVALID_STATE")
                            .SetMessage("The specified orchestration mutation is not valid for the current state of the game emulation.");
                    try
                    {
                        await gameEmulation.RestoreSaveGame();
                    }
                    catch (Exception e)
                    {
                        return ErrorBuilder.New()
                            .SetException(e)
                            .SetCode("ORCH_ERR_STARTEMULATION")
                            .SetMessage("An error occurred with starting this game emulation.");
                    }
                    return new EmulationInstancePayload()
                    {
                        GameEmulation = gameEmulation,
                        InstanceID = input.InstanceID,
                    };
                })
                .Type<NonNullType<EmulationInstancePayloadType>>();
            descriptor.Field("stopEmulation")
                .Description("Stops the specified emulation.")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Argument("input", arg => arg.Type<EmulationInstanceInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<EmulationInstanceInput>("input");
                    if (!ctx.GetGameCache().TryGetValue(input.InstanceID, out var gameEmulation))
                    {
                        return ErrorBuilder.New()
                            .SetCode("ORCH_INSTANCE_NOT_FOUND")
                            .SetMessage("The specified orchestration instance was not found.");
                    }
                    if (gameEmulation.EmulationState != GameEmulationState.CanStopEmulation)
                        return ErrorBuilder.New()
                            .SetCode("ORCH_INVALID_STATE")
                            .SetMessage("The specified orchestration mutation is not valid for the current state of the game emulation.");
                    try
                    {
                        await gameEmulation.RestoreSaveGame();
                    }
                    catch (Exception e)
                    {
                        return ErrorBuilder.New()
                            .SetException(e)
                            .SetCode("ORCH_ERR_STOPEMULATION")
                            .SetMessage("An error occurred with stopping this game emulation.");
                    }
                    return new EmulationInstancePayload()
                    {
                        GameEmulation = gameEmulation,
                        InstanceID = input.InstanceID,
                    };
                })
                .Type<NonNullType<EmulationInstancePayloadType>>();
            descriptor.Field("persistEmulationSave")
                .Description("Persists the current state of the emulation save file to the save profile.")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Argument("input", arg => arg.Type<EmulationInstanceInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<EmulationInstanceInput>("input");
                    if (!ctx.GetGameCache().TryGetValue(input.InstanceID, out var gameEmulation))
                    {
                        return ErrorBuilder.New()
                            .SetCode("ORCH_INSTANCE_NOT_FOUND")
                            .SetMessage("The specified orchestration instance was not found.");
                    }
                    if (gameEmulation.EmulationState != GameEmulationState.CanStartEmulation
                    && gameEmulation.EmulationState != GameEmulationState.RequiresDispose)
                        return ErrorBuilder.New()
                            .SetCode("ORCH_INVALID_STATE")
                            .SetMessage("The specified orchestration mutation is not valid for the current state of the game emulation.");
                    try
                    {
                        var save = await gameEmulation.PersistSaveGame();
                        return new PersistEmulationSavePayload()
                        {
                            GameEmulation = gameEmulation,
                            InstanceID = input.InstanceID,
                            SaveGame = save,
                        };
                    }
                    catch (Exception e)
                    {
                        return ErrorBuilder.New()
                            .SetException(e)
                            .SetCode("ORCH_ERR_PERSISTSAVEGAME")
                            .SetMessage("An error occurred with persisting the save profile for this emulation.");
                    }
                })
                .Type<NonNullType<PersistEmulationSavePayloadType>>();
        }
    }

    static class OrchestrationMutationContextExtesions
    {
        internal static ConcurrentDictionary<Guid, IGameEmulation> GetGameCache(this IResolverContext ctx)
             => (ConcurrentDictionary<Guid, IGameEmulation>)ctx.ContextData["Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration.GameInstanceCache"];
    }
}
