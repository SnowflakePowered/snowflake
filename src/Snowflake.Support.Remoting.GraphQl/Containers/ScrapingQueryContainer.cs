using System.Linq;
using Snowflake.Configuration;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Saving;
using Snowflake.Framework.Remoting.GraphQl;
using Snowflake.Input;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Queries;

namespace Snowflake.Support.Remoting.GraphQl.Containers
{
    public class ScrapingQueryContainer : IComposable
    {
        [ImportService(typeof(IGraphQlRootSchema))]
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IScrapeEngine<IGameRecord>))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var plugin = coreInstance.Get<IPluginManager>();
            var engine = coreInstance.Get<IScrapeEngine<IGameRecord>>();
            var rootSchema = coreInstance.Get<IGraphQlRootSchema>();
            var scrapeQuery = new ScrapingQueryBuilder(plugin.GetCollection<IScraper>(),
                plugin.GetCollection<ICuller>(), engine);
            rootSchema.Register(scrapeQuery);
            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Scraping GraphQL Queries.");
        }
    }
}
