using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Interface;
using SharpYaml.Serialization;

namespace Snowflake.API.Plugins
{
    public class Scraper: Plugin, IScraper
    {
        public string ScraperSource { get; private set; }
        public Dictionary<string, string> ScraperMap { get; private set; }
        public Scraper(string scraperName, string scraperSource, Dictionary<string, string> scraperMap) : base(scraperName)
        {
            this.ScraperSource = scraperSource;
            this.ScraperMap = scraperMap;
        }

        public Scraper(string scraperName, string scraperSource, string scraperMapYAML): base(scraperName)
        {
//todo implement yaml serializer
        }
    }
}
