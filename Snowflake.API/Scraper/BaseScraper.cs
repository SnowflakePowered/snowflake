using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Snowflake.Collections;
using Snowflake.Information.Game;
using Snowflake.Plugin;

namespace Snowflake.Scraper
{
    public abstract class BaseScraper: BasePlugin, IScraper
    {
        public BiDictionary<string, string> ScraperMap { get; private set; }

        protected BaseScraper(Assembly pluginAssembly) : base(pluginAssembly)
        {
            using (Stream stream = this.PluginAssembly.GetManifestResourceStream("scrapermap.json"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string file = reader.ReadToEnd();
                Console.WriteLine(file);
                BiDictionary<string, string> scraperMapValues = JsonConvert.DeserializeObject<BiDictionary<string, string>>(file);
                this.ScraperMap = scraperMapValues;
            }
            
        }
        public abstract IList<GameScrapeResult> GetSearchResults(string searchQuery);
        public abstract IList<GameScrapeResult> GetSearchResults(string searchQuery, string platformId);
        public abstract Tuple<IDictionary<string, string>, GameImages> GetGameDetails(string id);

    }
}
