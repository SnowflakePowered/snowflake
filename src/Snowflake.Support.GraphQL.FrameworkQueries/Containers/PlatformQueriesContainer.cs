using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Snowflake.Configuration;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.Model.Stone;
using Snowflake.Remoting.GraphQL.Schema;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Scraping;
using Snowflake.Services;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Game;
using Snowflake.Support.GraphQL.FrameworkQueries.Queries.Debug;
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

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class PlatformQueriesContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IGraphQLSchemaRegistrationProvider))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
          
            var hotChocolate = coreInstance.Get<IGraphQLSchemaRegistrationProvider>();
            var serviceRegistration = coreInstance.Get<IServiceRegistrationProvider>();

            hotChocolate.AddObjectTypeExtension<SubscriptionDebugQueries>();


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

            //hotChocolate.AddObjectTypeExtension<GameSubscriptions>();
            hotChocolate.AddObjectTypeExtension<ScrapingMutations>();
            //hotChocolate.AddObjectTypeExtension<AutoSubscriptions>();
            hotChocolate.ConfigureSchema(schema =>
            {
                schema.AddTypeInterceptor<AutoSubscriptionTypeInterceptor>();
            });

        }
    }
}
