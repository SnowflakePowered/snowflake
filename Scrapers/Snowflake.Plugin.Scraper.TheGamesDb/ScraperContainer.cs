using Snowflake.Caching;
using Snowflake.Extensibility;
using Snowflake.Scraper.Providers;
using Snowflake.Service;

namespace Snowflake.Plugin.Scraper.TheGamesDb
{
    public class ScraperContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            coreInstance.Get<IQueryProviderSource>().Register(new TheGamesDbMetadataProvider()); //todo use pluginManager?
            coreInstance.Get<IQueryProviderSource>()
                .Register(new TheGamesDbMediaProvider(new KeyedImageCache(coreInstance.AppDataDirectory)));
        }
    }
}
