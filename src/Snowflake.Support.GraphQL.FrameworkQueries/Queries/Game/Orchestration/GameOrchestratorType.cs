using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Configuration;
using Snowflake.Remoting.GraphQL.Model.Orchestration;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Orchestration.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;
using HotChocolate;
using Snowflake.Remoting.GraphQL;
using System.Linq;
using Snowflake.Input.Controller;
using Snowflake.Services;
using System.Threading.Tasks;
using Snowflake.Orchestration.Extensibility.Extensions;
using Snowflake.Input.Device;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Game.Orchestration
{
    public sealed class GameOrchestratorType
        : ObjectType<(IGame game, IEmulatorOrchestrator orchestrator)>
    {
        protected override void Configure(IObjectTypeDescriptor<(IGame game, IEmulatorOrchestrator orchestrator)> descriptor)
        {
            descriptor.Name("GameOrchestratorQuery")
                .Description("A query type that provides access to Game-dependent emulator orchestrator information.");

            descriptor.Field("pluginName")
                .Description("The name of the orchestrator plugin.")
                .Type<NonNullType<StringType>>()
                .Resolve(ctx => ctx.Parent<(IGame _, IEmulatorOrchestrator o)>().o.Name);
            descriptor.Field("compatibility")
                .Description("The compatibility level of the game with the emulator.")
                .Type<NonNullType<EmulatorCompatibilityEnum>>()
                .Resolve(ctx =>
                {
                    (IGame game, IEmulatorOrchestrator orchestrator) = ctx.Parent<(IGame, IEmulatorOrchestrator)>();
                    return orchestrator.CheckCompatibility(game);
                });
            descriptor.Field("missingSystemFiles")
               .Description("Any system files that are required but missing to run the game with this emulator.")
               .Type<NonNullType<ListType<NonNullType<SystemFileType>>>>()
               .Resolve(ctx =>
               {
                   (IGame game, IEmulatorOrchestrator orchestrator) = ctx.Parent<(IGame, IEmulatorOrchestrator)>();
                   return orchestrator.CheckMissingSystemFiles(game);
               });
            descriptor.Field("configurationProfiles")
               .Description("The names of configuration profiles saved with this emulator.")
               .Type<NonNullType<ListType<NonNullType<ConfigurationProfileType>>>>()
               .Resolve(ctx =>
               {
                   (IGame game, IEmulatorOrchestrator orchestrator) = ctx.Parent<(IGame, IEmulatorOrchestrator)>();
                   return orchestrator.GetConfigurationProfiles(game);
               });
            descriptor.Field("configuration")
                .Description("Retrieves the specified configuration profile for this orchestrator registered for this game.")
                .Argument("collectionId", arg => arg.Description("The collectionId of the profile to retrieve").Type<NonNullType<UuidType>>())
                .Type<ConfigurationCollectionType>()
                .Resolve(ctx =>
                {
                    (IGame game, IEmulatorOrchestrator orchestrator) = ctx.Parent<(IGame, IEmulatorOrchestrator)>();
                    Guid configProfile = ctx.ArgumentValue<Guid>("collectionId");
                    return orchestrator.GetGameConfiguration(game, configProfile);
                });
            descriptor.Field("ports")
                .Description("Retrieves the emulated ports of this orchestrator for the platform of the game. If a retrieved port " +
                "does not exist, it is unassigned.")
                .Argument("portIndex", arg => arg
                    .Description("The port index to retrieve, if retrieving a specific port.")
                    .Type<IntType>())
                .Resolve(ctx =>
                {
                    (IGame game, IEmulatorOrchestrator orchestrator) = ctx.Parent<(IGame, IEmulatorOrchestrator)>();
                    var portStore = ctx.SnowflakeService<IEmulatedPortStore>();
                    var arg = ctx.ArgumentValue<Optional<int>>("portIndex");
                    IEnumerable<IEmulatedPortDeviceEntry> deviceEntries;
                    if (arg.HasValue)
                    {
                        var port = portStore.GetPort(orchestrator, game.Record.PlatformID, arg.Value);
                        deviceEntries = port != null ? new[] { port } : Enumerable.Empty<IEmulatedPortDeviceEntry>();
                    }
                    else
                    {
                        deviceEntries = portStore.EnumeratePorts(orchestrator, game.Record.PlatformID);
                    }
                    return deviceEntries;
                })
                .Type<NonNullType<ListType<NonNullType<ConnectedEmulatedControllerType>>>>();
        }
    }
}
