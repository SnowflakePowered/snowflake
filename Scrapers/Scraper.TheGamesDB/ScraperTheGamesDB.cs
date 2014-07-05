using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Net;
using Snowflake.API.Base.Scraper;
using System.Xml.Linq;

namespace Scraper.TheGamesDB
{
    public class ScraperTheGamesDB : BaseScraper
    {
        public ScraperTheGamesDB()

            : base("thegamesdb", "thegamesdb.net", Assembly.GetExecutingAssembly())
        {
        }

        private List<GameScrapeResult> ParseSearchResults(Uri searchUri)
        {
            Console.WriteLine(searchUri.OriginalString);
            using (WebClient client = new WebClient())
            {
                string xml = client.DownloadString(searchUri);
                XDocument xmlDoc = XDocument.Parse(xml);
                var games = (from game in xmlDoc.Descendants("Game") select game).ToList();
                var results = new List<GameScrapeResult>();
                foreach (var game in games)
                {
                    var id = game.Elements("id").First().Value;
                    var title = game.Elements("GameTitle").First().Value;
                    string platform = "SNOWFLAKE_UNKNOWN";
                    string xmlPlatformValue = game.Elements("Platform").First().Value;
                    if (this.ScraperMap.Reverse.ContainsKey(xmlPlatformValue)) platform = this.ScraperMap.Reverse[xmlPlatformValue];

                    results.Add(new GameScrapeResult(id, platform, title));
                }
                return results;
            }
        }
        public override List<GameScrapeResult> GetSearchResults(string searchQuery)
        {
            Uri searchUri = new Uri(Uri.EscapeUriString("http://thegamesdb.net/api/GetGamesList.php?name=" + searchQuery));
            var results = ParseSearchResults(searchUri);
            return results;
        }

        public override List<GameScrapeResult> GetSearchResults(string searchQuery, string platformId)
        {
            Uri searchUri = new Uri(Uri.EscapeUriString("http://thegamesdb.net/api/GetGamesList.php?name=" + searchQuery
                + "&platform=" + this.ScraperMap[platformId]));
            var results = ParseSearchResults(searchUri);
            return results;
        }

        public override Tuple<Dictionary<string, string>, Dictionary<string,string>> GetGameDetails(string id)
        {
            Uri searchUri = new Uri(Uri.EscapeUriString("http://thegamesdb.net/api/GetGame.php?id=" + id));
            Console.WriteLine(searchUri.AbsoluteUri);
            using (WebClient client = new WebClient())
            {
                string xml = client.DownloadString(searchUri);
                XDocument xmlDoc = XDocument.Parse(xml);
                string baseImageUrl = xmlDoc.Descendants("baseImgUrl").First().Value;
                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("SNOWFLAKE_GAME_DESCRIPTION",
                    xmlDoc.Descendants("Overview").First().Value);
                metadata.Add("SNOWFLAKE_GAME_TITLE",
                   xmlDoc.Descendants("GameTitle").First().Value);
                metadata.Add("SNOWFLAKE_GAME_RELEASEDATE",
                   xmlDoc.Descendants("ReleaseDate").First().Value);
                metadata.Add("SNOWFLAKE_GAME_PUBLISHER",
                   xmlDoc.Descendants("Publisher").First().Value);
                metadata.Add("SNOWFLAKE_GAME_DEVELOPER",
                   xmlDoc.Descendants("Developer").First().Value);

                Dictionary<string, string> images = new Dictionary<string, string>();
                //todo add images
                return Tuple.Create(metadata, images);

            }

        }
    }
}
