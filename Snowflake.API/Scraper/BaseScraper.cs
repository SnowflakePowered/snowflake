using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Snowflake.Collections;
using Snowflake.Plugin;
using Snowflake.Core;
namespace Snowflake.Scraper
{
    public abstract class BaseScraper: BasePlugin, IScraper
    {
        public BiDictionary<string, string> ScraperMap { get; private set; }

        protected BaseScraper(Assembly pluginAssembly, FrontendCore coreInstance) : base(pluginAssembly, coreInstance)
        {
            using (Stream stream = this.PluginAssembly.GetManifestResourceStream("scrapermap.json"))
            using (var reader = new StreamReader(stream))
            {
                string file = reader.ReadToEnd();
                Console.WriteLine(file);
                var scraperMapValues = JsonConvert.DeserializeObject<BiDictionary<string, string>>(file);
                this.ScraperMap = scraperMapValues;
            }
            
        }
        public abstract IList<GameScrapeResult> GetSearchResults(string searchQuery);
        public abstract IList<GameScrapeResult> GetSearchResults(string searchQuery, string platformId);
        public abstract Tuple<Dictionary<string, string>, GameImagesResult> GetGameDetails(string id);

    }
}
