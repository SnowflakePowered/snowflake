using Snowflake.Extensibility;
using Snowflake.Scraper.Providers;
using Snowflake.Service;

namespace Snowflake.Scrapers.TheGamesDb
{
    public class ScraperContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            coreInstance.Get<IQueryProviderSource>().Register(new TheGamesDbMetadataProvider());
        }
    }
}
