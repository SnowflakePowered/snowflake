using System.Linq;
using Snowflake.Configuration;
using Snowflake.Input;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework;
using Snowflake.Support.Remoting.GraphQl.Queries;

namespace Snowflake.Support.Remoting.GraphQl
{
    public class GraphQlContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IGameLibrary))]
        [ImportService(typeof(IConfigurationCollectionStore))]
        [ImportService(typeof(IGraphQlRootSchema))]
        [ImportService(typeof(IInputManager))]
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IMappedControllerElementCollectionStore))]
        [ImportService(typeof(IScrapeEngine<IGameRecord>))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var stone = coreInstance.Get<IStoneProvider>();
            var games = coreInstance.Get<IGameLibrary>();
            var config = coreInstance.Get<IConfigurationCollectionStore>();
            var input = coreInstance.Get<IInputManager>();
            var plugin = coreInstance.Get<IPluginManager>();
            var mapp = coreInstance.Get<IMappedControllerElementCollectionStore>();
            var engine = coreInstance.Get<IScrapeEngine<IGameRecord>>();

            var rootSchema = coreInstance.Get<IGraphQlRootSchema>();
            var platformQueries = new PlatformInfoQueryBuilder(stone);
            var controllerQueries = new ControllerLayoutQueryBuilder(stone);
            var recordQueries = new RecordQueryBuilder(games, stone);
            var configQuery = new ConfigurationQueryBuilder(config);

            var inputQuery = new InputQueryBuilder(input, plugin, mapp, stone);

            var scrapeQuery = new ScrapingQueryBuilder(plugin.GetCollection<IScraper>(), plugin.GetCollection<ICuller>(), engine);
            rootSchema.Register(platformQueries);
            rootSchema.Register(controllerQueries);
            rootSchema.Register(recordQueries);
            rootSchema.Register(configQuery);
            rootSchema.Register(inputQuery);
            rootSchema.Register(scrapeQuery);
        }
    }
}
