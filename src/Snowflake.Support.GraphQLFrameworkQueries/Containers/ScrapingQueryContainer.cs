using System.Linq;
using Snowflake.Configuration;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Saving;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Input;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Model.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQL.Queries;

namespace Snowflake.Support.Remoting.GraphQL.Containers
{
    public class ScrapingQueryContainer : IComposable
    {
        [ImportService(typeof(IGraphQLService))]
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IScrapeEngine<IGameRecord>))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var plugin = coreInstance.Get<IPluginManager>();
            var engine = coreInstance.Get<IScrapeEngine<IGameRecord>>();
            var rootSchema = coreInstance.Get<IGraphQLService>();
            var scrapeQuery = new ScrapingQueryBuilder(plugin.GetCollection<IScraper>(),
                plugin.GetCollection<ICuller>(), engine);
            rootSchema.Register(scrapeQuery);
            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Scraping GraphQL Queries.");
        }
    }
}
