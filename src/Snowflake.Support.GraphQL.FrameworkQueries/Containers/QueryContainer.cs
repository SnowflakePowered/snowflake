﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using Snowflake.Remoting.GraphQL;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Scraping;
using Snowflake.Services;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Game;
using Snowflake.Support.GraphQL.FrameworkQueries.Queries.Game.Orchestration;
using Snowflake.Support.GraphQLFrameworkQueries.Queries;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices.Mapped;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Filesystem;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Game;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Installables;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.LibraryExtensions;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Runtime;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Stone;
using Snowflake.Support.GraphQL.FrameworkQueries.Queries.Queueing;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using HotChocolate.Types.Descriptors.Definitions;
using HotChocolate.Types.Descriptors;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
using HotChocolate.Language;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Scraping;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation;
using System.Collections.Concurrent;
using System;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Installation;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Input;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Configuration;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Saving;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Ports;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Orchestration;
using HotChocolate.Execution.Configuration;
using HotChocolate.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class QueryContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IGraphQLSchemaRegistrationProvider))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var hotChocolate = coreInstance.Get<IGraphQLSchemaRegistrationProvider>();
            this.ConfigureHotChocolate(hotChocolate);
        }

        internal void ConfigureHotChocolate(IGraphQLSchemaRegistrationProvider hotChocolate)
        {

            hotChocolate.AddObjectTypeExtension<StoneQueries>();
            hotChocolate.AddObjectTypeExtension<GameQueries>();
            hotChocolate.AddObjectTypeExtension<GameFileQueries>();
            hotChocolate.AddObjectTypeExtension<GameNodeQueries>();
            hotChocolate.AddObjectTypeExtension<DeviceQueries>();
            hotChocolate.AddObjectTypeExtension<DeviceNodeQueries>();
            hotChocolate.AddObjectTypeExtension<PlatformInfoNodeQueries>();
            hotChocolate.AddObjectTypeExtension<ControllerLayoutNodeQueries>();

            hotChocolate.AddEmptyQueryType("runtime", "RuntimeQuery", "Provides access to Snowflake runtime details.");
            hotChocolate.AddObjectTypeExtension<RuntimeQueries>();

            hotChocolate.AddObjectTypeExtension<FilesystemQueries>();

            hotChocolate.AddInterfaceTypeExtension<InstallableQueries>();
            hotChocolate.AddObjectTypeExtension<DirectoryInfoInstallableQueries>();
            hotChocolate.AddObjectTypeExtension<DriveInfoInstallableQueries>();

            hotChocolate.AddObjectTypeExtension<InputDeviceInstanceMappingProfileQueries>();

            hotChocolate.AddObjectTypeExtension<SaveQueries>();

            hotChocolate.AddObjectTypeExtension<PluginQueries>();
            hotChocolate.AddObjectTypeExtension<GameOrchestrationQueries>();

            hotChocolate.AddObjectTypeExtension<GameMutations>();
            hotChocolate.AddObjectTypeExtension<GameMetadataMutations>();

            hotChocolate.AddEmptyQueryType("job", "JobQuery", "Provides access to Snowflake runtime details.");
            hotChocolate.AddObjectTypeExtension<JobQueries>();

            hotChocolate.AddObjectTypeExtension<ScrapingMutations>();
            hotChocolate.AddObjectTypeExtension<ScrapingSubscriptions>();
            hotChocolate.AddObjectType<ScrapeContextCompletePayloadType>();
            hotChocolate.AddObjectType<ScrapeContextStepPayloadType>();
            hotChocolate.AddInterfaceType<ScrapeContextPayloadInterface>();

            hotChocolate.AddObjectTypeExtension<InstallationMutations>();
            hotChocolate.AddObjectTypeExtension<InstallationSubscriptions>();

            hotChocolate.AddObjectTypeExtension<InputMutations>();
            hotChocolate.AddObjectTypeExtension<ConfigurationMutations>();

            hotChocolate.AddObjectTypeExtension<SavingMutations>();

            hotChocolate.ConfigureSchema(schema =>
            {
                schema.TryAddTypeInterceptor<AutoSubscriptionTypeInterceptor>();
            });

            hotChocolate.AddObjectTypeExtension<OrchestrationMutations>();
            hotChocolate.AddObjectTypeExtension<PortMutations>();

            hotChocolate.AddObjectTypeExtension<OrchestrationSubscriptions>();

            var jobQueue = new ConcurrentDictionary<Guid, Guid>();
            var gameInstanceCache = new ConcurrentDictionary<Guid, IGameEmulation>();
            
            hotChocolate.ConfigureQueryRequest(query =>
            {
                query.SetProperty("Snowflake.Support.GraphQL.FrameworkQueries.ResolverJobQueueMetadata.Store", jobQueue);
                query.SetProperty("Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration.GameInstanceCache", gameInstanceCache);
            });
        }
    }
}
