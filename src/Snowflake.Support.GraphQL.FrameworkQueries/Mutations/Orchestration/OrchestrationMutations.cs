using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Saving;
using Snowflake.Remoting.GraphQL;
using Snowflake.Services;
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
            descriptor.Extend()
                .OnBeforeCreate(defn =>
                {
                    defn.ContextData.Add("Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration.GameInstanceCache",
                        new ConcurrentDictionary<Guid, IGameEmulation>());
                });
            descriptor.Field("createEmulationInstance")
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
                        switch(compatibility)
                        {
                            case EmulatorCompatibility.MissingSystemFiles:
                                return ErrorBuilder.New()
                                    .SetCode("GAME_MISSING_SYSTEM_FILES")
                                    .SetMessage("The specified game can not be run. The emulator requires system files that could not be found.");
                            case EmulatorCompatibility.RequiresValidation:
                                return ErrorBuilder.New()
                                    .SetCode("GAME_REQUIRES_VALIDATION")
                                    .SetMessage("The specified game must be validated before it can run.");
                            case EmulatorCompatibility.Unsupported:
                                return ErrorBuilder.New()
                                    .SetCode("GAME_UNSUPPORTED")
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
                            Emulation = instance,
                            InstanceID = guid
                        };
                    throw new InvalidOperationException("Could not create emulation instance.");
                }).Type<NonNullType<EmulationInstancePayloadType>>();
            //descriptor.Field("prepareEmulationInstance")
            //    .Resolver(ctx =>
            //    {

            //    });
        }
    }

    static class OrchestrationMutationContextExtesions
    {
       internal static ConcurrentDictionary<Guid, IGameEmulation> GetGameCache(this IResolverContext ctx)
            => (ConcurrentDictionary<Guid, IGameEmulation>)ctx.RootType.ContextData["Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration.GameInstanceCache"];
    }
}
