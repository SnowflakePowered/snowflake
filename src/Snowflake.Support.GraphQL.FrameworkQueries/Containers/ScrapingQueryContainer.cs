using System.Linq;
using Snowflake.Remoting.GraphQL;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Queries;

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class ScrapingQueryContainer : IComposable
    {
        [ImportService(typeof(IGraphQLService))]
        [ImportService(typeof(IGameLibrary))]
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var plugin = coreInstance.Get<IPluginManager>();
            var lib = coreInstance.Get<IGameLibrary>();
            var rootSchema = coreInstance.Get<IGraphQLService>();
            var scrapeQuery = new ScrapingQueryBuilder(lib, 
                plugin.GetCollection<IScraper>(),
                plugin.GetCollection<ICuller>(),
                plugin.GetCollection<IGameMetadataTraverser>(),
                plugin.GetCollection<IFileInstallationTraverser>());
            rootSchema.Register(scrapeQuery);
            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Scraping GraphQL Queries.");
        }
    }
}
