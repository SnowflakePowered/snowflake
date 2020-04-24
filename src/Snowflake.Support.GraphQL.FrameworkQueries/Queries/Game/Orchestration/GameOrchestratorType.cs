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
                .Resolver(ctx => ctx.Parent<(IGame _, IEmulatorOrchestrator o)>().o.Name);
            descriptor.Field("compatibility")
                .Description("The compatibility level of the game with the emulator.")
                .Type<NonNullType<EmulatorCompatibilityEnum>>()
                .Resolver(ctx =>
                {
                    (IGame game, IEmulatorOrchestrator orchestrator) = ctx.Parent<(IGame, IEmulatorOrchestrator)>();
                    return orchestrator.CheckCompatibility(game);
                });
            descriptor.Field("missingSystemFiles")
               .Description("Any system files that are required but missing to run the game with this emulator.")
               .Type<NonNullType<ListType<NonNullType<SystemFileType>>>>()
               .Resolver(ctx =>
               {
                   (IGame game, IEmulatorOrchestrator orchestrator) = ctx.Parent<(IGame, IEmulatorOrchestrator)>();
                   return orchestrator.CheckMissingSystemFiles(game);
               });
            descriptor.Field("configurationProfiles")
               .Description("The names of configuration profiles saved with this emulator.")
               .Type<NonNullType<ListType<StringType>>>()
               .Resolver(ctx =>
               {
                   (IGame game, IEmulatorOrchestrator orchestrator) = ctx.Parent<(IGame, IEmulatorOrchestrator)>();
                   return orchestrator.GetConfigurationProfiles(game);
               });
            descriptor.Field("configuration")
                .Description("Retrieves the specified configuration profile for this orchestrator registered for this game.")
                .Argument("profileName", arg => arg.Description("The name of the profile to retrieve").Type<NonNullType<StringType>>())
                .Type<ConfigurationCollectionType>()
                .Resolver(ctx =>
                {
                    (IGame game, IEmulatorOrchestrator orchestrator) = ctx.Parent<(IGame, IEmulatorOrchestrator)>();
                    string configProfile = ctx.Argument<string>("profileName");
                    return orchestrator.GetGameConfiguration(game, configProfile);
                });
        }
    }
}
