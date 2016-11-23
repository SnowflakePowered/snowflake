using Snowflake.Caching;
using Snowflake.Extensibility;
using Snowflake.Scraper.Providers;
using Snowflake.Services;

namespace Snowflake.Plugin.Scrapers.TheGamesDb
{
    public class ScraperContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            coreInstance.Get<IQueryProviderSource>().Register(new TheGamesDbMetadataProvider()); //todo use pluginManager with encapsulated interdfaces?
            coreInstance.Get<IQueryProviderSource>()
                .Register(new TheGamesDbMediaProvider(new KeyedImageCache(coreInstance.AppDataDirectory)));
        }
    }
}
