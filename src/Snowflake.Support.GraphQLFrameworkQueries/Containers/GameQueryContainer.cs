using System.Linq;
using Snowflake.Configuration;
using Snowflake.Extensibility.Configuration;
using Snowflake.Filesystem;
using Snowflake.Framework.Extensibility;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Input;
using Snowflake.Input.Device;
using Snowflake.Installation;
using Snowflake.Installation.Extensibility;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Queries;

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class GameQueryContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IGameLibrary))]
        [ImportService(typeof(IGraphQLService))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var plugin = coreInstance.Get<IPluginManager>();

            var games = coreInstance.Get<IGameLibrary>();
            var rootSchema = coreInstance.Get<IGraphQLService>();

            var gameQuery = new GameQueryBuilder(plugin.GetCollection<IGameInstaller>(),
                new AsyncJobQueue<TaskResult<IFile>>(), games);
            rootSchema.Register(gameQuery);
            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Game GraphQL Queries.");
        }
    }
}
