using System.Linq;
using HotChocolate.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Configuration;
using Snowflake.Extensibility.Configuration;
using Snowflake.Filesystem;
using Snowflake.Remoting.GraphQL;
using Snowflake.Input;
using Snowflake.Input.Device;
using Snowflake.Installation;
using Snowflake.Installation.Extensibility;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Queries;
using Snowflake.Extensibility.Queueing;

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class InstallationQueryContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IGameLibrary))]
        [ImportService(typeof(IGraphQLService))]
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IAsyncJobQueueFactory))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var plugin = coreInstance.Get<IPluginManager>();

            var games = coreInstance.Get<IGameLibrary>();
            var rootSchema = coreInstance.Get<IGraphQLService>();
            var jobQueue = coreInstance.Get<IAsyncJobQueueFactory>();
            
            var gameQuery = new InstallationQueryBuilder(plugin.GetCollection<IGameInstaller>(),
                plugin.GetCollection<IEmulatorOrchestrator>(),
                jobQueue.GetJobQueue<TaskResult<IFile>>(), games);
            rootSchema.Register(gameQuery);
            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Game GraphQL Queries.");
        }
    }
}
