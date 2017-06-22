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
        public void Compose(IServiceContainer coreInstance)
        {
            var appdata = coreInstance.Get<IContentDirectoryProvider>();
            coreInstance.Get<IQueryProviderSource>().Register(new TheGamesDbMetadataProvider()); //todo use pluginManager with encapsulated interdfaces?
            coreInstance.Get<IQueryProviderSource>()
                .Register(new TheGamesDbMediaProvider(new KeyedImageCache(appdata.ApplicationData.FullName)));
        }
    }
}
