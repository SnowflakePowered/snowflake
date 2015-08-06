using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Snowflake.Identifier;
using Snowflake.Plugin;
using Snowflake.Service;
using Snowflake.Utility;

namespace Snowflake.Scraper
{
    public abstract class BaseScraper: BasePlugin, IScraper
    {
        public BiDictionary<string, string> ScraperMap { get; }

        protected BaseScraper(Assembly pluginAssembly, ICoreService coreInstance) : base(pluginAssembly, coreInstance)
        {
            using (Stream stream = this.GetResource("scrapermap.json"))
            using (var reader = new StreamReader(stream))
            {
                string file = reader.ReadToEnd();
                var scraperMapValues = JsonConvert.DeserializeObject<BiDictionary<string, string>>(file);
                this.ScraperMap = scraperMapValues;
            }
            
        }
        public abstract IList<IGameScrapeResult> GetSearchResults(string searchQuery);
        public abstract IList<IGameScrapeResult> GetSearchResults(string searchQuery, string platformId);
        public abstract IList<IGameScrapeResult> GetSearchResults(IList<IIdentifiedMetadata> identifiedMetadata, string platformId);
        public abstract IList<IGameScrapeResult> GetSearchResults(IList<IIdentifiedMetadata> identifiedMetadata, string searchQuery, string platformId);
        public abstract IList<IGameScrapeResult> SortBestResults(IList<IIdentifiedMetadata> identifiedMetadata, IList<IGameScrapeResult> searchResults);
        public abstract Tuple<IDictionary<string, string>, IGameImagesResult> GetGameDetails(string id);

    }
}
