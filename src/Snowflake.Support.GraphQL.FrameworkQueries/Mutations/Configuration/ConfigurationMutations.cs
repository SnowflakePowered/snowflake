using HotChocolate;
using HotChocolate.Types;
using Snowflake.Configuration;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Services;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Configuration
{
    public sealed class ConfigurationMutations
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Mutation");
            descriptor.Field("updateGameConfigurationValues")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Description("Updates the provided configuration values.")
                .Argument("input", a => a.Type<UpdateGameConfigurationValueInputType>())
                .Resolver(async ctx =>
                {
                    var configStore = ctx.SnowflakeService<IGameLibrary>().GetExtension<IGameConfigurationExtensionProvider>();
                    var input = ctx.Argument<UpdateGameConfigurationValueInput>("input");
                    var newValues = new List<IConfigurationValue>();
                    var newValueGuid = new List<(Guid valueCollection, Guid value)>();
                    foreach (var value in input.Values)
                    {
                        var owningGuid = await configStore.GetOwningValueCollectionAsync(value.ValueID);
                        if (owningGuid == default) continue; // value is an orphan or not found.

                        await configStore.UpdateValueAsync(value.ValueID, value.Value);
                        newValues.Add(new ConfigurationValue(value.Value, value.ValueID));
                        newValueGuid.Add((owningGuid, value.ValueID));
                    }
                    return new UpdateGameConfigurationValuePayload()
                    {
                        Values = newValues,
                        Collections = newValueGuid.GroupBy(k => k.valueCollection, v => v.value),
                    };
                }).Type<NonNullType<UpdateGameConfigurationValuePayloadType>>();
            descriptor.Field("deleteGameConfiguration")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Description("Delete the specified game configuration profile.")
                .Argument("input", a => a.Type<DeleteGameConfigurationInputType>())
                .Resolver(async ctx =>
                {
                    var games = ctx.SnowflakeService<IGameLibrary>();
                    var configStore = games.GetExtension<IGameConfigurationExtensionProvider>();
                    var orchestrators = ctx.SnowflakeService<IPluginManager>().GetCollection<IEmulatorOrchestrator>();
                    var input = ctx.Argument<DeleteGameConfigurationInput>("input");
                    IConfigurationCollection config = null;

                    if (input.Retrieval != null)
                    {
                        var orchestrator = orchestrators[input.Retrieval.Orchestrator];
                        var game = await games.GetGameAsync(input.Retrieval.GameID);
                        if (orchestrator == null)
                            return ErrorBuilder.New()
                                .SetCode("CFG_NOTFOUND_ORCHESTRATOR")
                                .SetMessage("The specified orchestrator was not found.")
                                .Build();
                        if (game == null)
                            return ErrorBuilder.New()
                                .SetCode("CFG_NOTFOUND_GAME")
                                .SetMessage("The specified game was not found.")
                                .Build();
                        config = orchestrator.GetGameConfiguration(game, input.CollectionID);
                        if (config == null)
                            return ErrorBuilder.New()
                                .SetCode("CFG_NOTFOUND_COLLECTION")
                                .SetMessage("The specified collectionId was not found in the configuration set for the specified orchestrator..")
                                .Build();
                    }

                    await configStore.DeleteProfileAsync(input.CollectionID);

                    return new DeleteGameConfigurationPayload()
                    {
                        CollectionID = input.CollectionID,
                        Configuration = config,
                    };
                }).Type<NonNullType<DeleteGameConfigurationPayloadType>>();
            descriptor.Field("updatePluginConfigurationValues")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Description("Updates .")
                .Argument("input", a => a.Type<UpdatePluginConfigurationValueInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<UpdatePluginConfigurationValueInput>("input");
                    var plugin = ctx.SnowflakeService<IPluginManager>().Get(input.Plugin);
                    if (plugin == null)
                        return ErrorBuilder.New()
                                .SetCode("PCFG_NOTFOUND_PLUGIN")
                                .SetMessage("The specified plugin was not found.")
                                .Build();
                    var configStore = plugin?.Provision?.ConfigurationStore;
                    if (configStore == null)
                        return ErrorBuilder.New()
                                .SetCode("PCFG_NOTPROVISIONED")
                                .SetMessage("The specified plugin was not a provisioned plugin.")
                                .Build();
                    var newValues = new List<Guid>();
                    foreach (var value in input.Values)
                    {
                        await configStore.SetAsync(new ConfigurationValue(value.Value, value.ValueID));
                        newValues.Add(value.ValueID);
                    }

                    var configuration = plugin.GetPluginConfiguration();
                    return new UpdatePluginConfigurationValuePayload()
                    {
                        Values = newValues.Select(g => configuration.ValueCollection[g].value).Where(v => v != null).ToList(),
                        Plugin = plugin,
                    };
                }).Type<NonNullType<UpdatePluginConfigurationValuePayloadType>>();
        }
    }
}
