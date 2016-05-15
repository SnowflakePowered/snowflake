using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;
using Snowflake.Utility;

namespace Snowflake.Scraper
{
    public abstract class Scraper: Extensibility.Plugin, IScraper
    {
        public IDictionary<string, string> ScraperMap { get; }
        public double ScraperAccuracy { get; }

        protected Scraper(ICoreService coreInstance) : base(coreInstance)
        {
            using (Stream stream = this.GetResource("scrapermap.json"))
            using (var reader = new StreamReader(stream))
            {
                string file = reader.ReadToEnd();
                double accuracy = Convert.ToDouble(this.PluginProperties.Get("scraper_accuracy"));
                var scraperMapValues = JsonConvert.DeserializeObject<IDictionary<string, string>>(file);
                this.ScraperMap = scraperMapValues;
                this.ScraperAccuracy = accuracy > 1.0
                    ? 1.0
                    : accuracy;
            }
        }
        public abstract IList<IGameScrapeResult> GetSearchResults(string searchQuery);
        public abstract IList<IGameScrapeResult> GetSearchResults(string searchQuery, string platformId);
        public abstract IList<IGameScrapeResult> GetSearchResults(IScrapableInfo scrapableInfo);
        public abstract Tuple<IDictionary<string, string>, IGameImagesResult> GetGameDetails(string id);
        public abstract Tuple<IDictionary<string, string>, IGameImagesResult> GetGameDetails(IGameScrapeResult gameScrapeResult);


    }
}
