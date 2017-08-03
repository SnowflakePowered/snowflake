using Snowflake.Caching;
using Snowflake.Extensibility;
using Snowflake.Loader;
using Snowflake.Scraper.Providers;
using Snowflake.Services;

namespace Snowflake.Plugin.Scrapers.TheGamesDb
{
    public class ScraperContainer : IComposable
    {
        [ImportService(typeof(IQueryProviderSource))]
        [ImportService(typeof(IContentDirectoryProvider))]
        [ImportService(typeof(IKeyedImageCache))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var appdata = serviceContainer.Get<IContentDirectoryProvider>();
            serviceContainer.Get<IQueryProviderSource>()
                .Register(new TheGamesDbMetadataProvider()); //todo use pluginManager with encapsulated interdfaces?
            serviceContainer.Get<IQueryProviderSource>()
                .Register(new TheGamesDbMediaProvider(serviceContainer.Get<IKeyedImageCache>()));
        }
    }
}
