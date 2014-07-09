using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Interface;
using Newtonsoft.Json;
using Snowflake.API.Collections;
using System.Reflection;
using System.IO;
using Snowflake.API.Information.Game;

namespace Snowflake.API.Base.Scraper
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
        public abstract List<GameScrapeResult> GetSearchResults(string searchQuery);
        public abstract List<GameScrapeResult> GetSearchResults(string searchQuery, string platformId);
        public abstract Tuple<IDictionary<string, string>, GameImages> GetGameDetails(string id);

    }
}
