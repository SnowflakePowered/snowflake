using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Plugin.Interface;
using Newtonsoft.Json;
using Snowflake.Collections;
using System.Reflection;
using System.IO;
using Snowflake.Information.Platform;
using Snowflake.Information.Game;

namespace Snowflake.Plugin.Scraper
{
    public abstract class BaseScraper: BasePlugin, IScraper
    {
        public BiDictionary<string, string> ScraperMap { get; private set; }

        public BaseScraper(Assembly pluginAssembly) : base(pluginAssembly)
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
