using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tests.Fakes
{
    internal class FakeScraper : Snowflake.Scraper.IScraper
    {
        public Utility.BiDictionary<string, string> ScraperMap
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Scraper.IGameScrapeResult> GetSearchResults(string searchQuery)
        {
            throw new NotImplementedException();
        }

        public IList<Scraper.IGameScrapeResult> GetSearchResults(string searchQuery, string platformId)
        {
            throw new NotImplementedException();
        }

        public Tuple<IDictionary<string, string>, Scraper.IGameImagesResult> GetGameDetails(string id)
        {
            throw new NotImplementedException();
        }

        public string PluginName
        {
            get { throw new NotImplementedException(); }
        }

        public string PluginDataPath
        {
            get { throw new NotImplementedException(); }
        }

        public IList<string> SupportedPlatforms
        {
            get { throw new NotImplementedException(); }
        }

        public System.Reflection.Assembly PluginAssembly
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, dynamic> PluginInfo
        {
            get { throw new NotImplementedException(); }
        }

        public Service.ICoreService CoreInstance
        {
            get { throw new NotImplementedException(); }
        }

        public System.IO.Stream GetResource(string resourceName)
        {
            throw new NotImplementedException();
        }

        public string GetStringResource(string resourceName)
        {
            throw new NotImplementedException();
        }

        public Plugin.IPluginConfiguration PluginConfiguration
        {
            get { throw new NotImplementedException(); }
        }
    }
}
