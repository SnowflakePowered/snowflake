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

namespace Snowflake.API.Plugins.Scraper
{
    public abstract class BaseScraper: Plugin, IScraper
    {
        public string ScraperSource { get; private set; }
        public BiDictionary<string, string> ScraperMap { get; private set; }
        public BaseScraper(string scraperName, string scraperSource, BiDictionary<string, string> scraperMap) : base(scraperName)
        {
            this.ScraperSource = scraperSource;
            this.ScraperMap = scraperMap;
        }

        public BaseScraper(string scraperName, string scraperSource, Assembly scraperAssembly): base(scraperName)
        {

            this.ScraperSource = scraperSource;
            using (Stream stream = scraperAssembly.GetManifestResourceStream("scrapermap.json"))
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
        public abstract Tuple<Dictionary<string, string>, Dictionary<string, string>> GetGameDetails(string id);

    }
}
