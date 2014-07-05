using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Net;
using Snowflake.API;
using Snowflake.API.Base.Scraper;
using Snowflake.API.Information.Game;
using System.Xml.Linq;
using Snowflake.API.Constants;

namespace Scraper.TheGamesDB
{
    public class ScraperTheGamesDB : BaseScraper
    {
        public ScraperTheGamesDB():base(Assembly.GetExecutingAssembly())
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
                    var id = game.Element("id").Value;
                    var title = game.Element("GameTitle").Value;
                    string platform = "SNOWFLAKE_UNKNOWN";
                    string xmlPlatformValue = game.Element("Platform").Value;
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
        public override Tuple<Dictionary<string, string>, GameImages> GetGameDetails(string id)
        {
            Uri searchUri = new Uri(Uri.EscapeUriString("http://thegamesdb.net/api/GetGame.php?id=" + id));
            Console.WriteLine(searchUri.AbsoluteUri);
            using (WebClient client = new WebClient())
            {
                string xml = client.DownloadString(searchUri);
                XDocument xmlDoc = XDocument.Parse(xml);
                string baseImageUrl = xmlDoc.Descendants("baseImgUrl").First().Value;
                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add(GameInfoFields.snowflake_game_description,
                    xmlDoc.Descendants("Overview").First().Value);
                metadata.Add(GameInfoFields.snowflake_game_title,
                   xmlDoc.Descendants("GameTitle").First().Value);
                metadata.Add(GameInfoFields.snowflake_game_releasedate,
                   xmlDoc.Descendants("ReleaseDate").First().Value);
                metadata.Add(GameInfoFields.snowflake_game_publisher,
                   xmlDoc.Descendants("Publisher").First().Value);
                metadata.Add(GameInfoFields.snowflake_game_developer,
                   xmlDoc.Descendants("Developer").First().Value);

                GameImages images = new GameImages();
                var boxartFront = baseImageUrl + (from boxart in xmlDoc.Descendants("boxart") where boxart.Attribute("side").Value == "front" select boxart).First().Value;
                images.AddFromUrl(GameImageType.Boxart_front, new Uri(boxartFront));

                var boxartBack = baseImageUrl + (from boxart in xmlDoc.Descendants("boxart") where boxart.Attribute("side").Value == "back" select boxart).First().Value;
                images.AddFromUrl(GameImageType.Boxart_back, new Uri(boxartBack));

                //Add fanarts
                var fanarts = (from fanart in xmlDoc.Descendants("fanart") select fanart).ToList();
                foreach (var fanart in fanarts)
                {
                    string fanartUrl = baseImageUrl + fanart.Element("original").Value;
                    images.AddFromUrl(GameImageType.Fanart, new Uri(fanartUrl));
                }

                //Add screenshots
                var screenshots = (from screenshot in xmlDoc.Descendants("screenshot") select screenshot).ToList();
                foreach (var screenshot in screenshots)
                {
                    string screenshotUrl = baseImageUrl + screenshot.Element("original").Value;
                    images.AddFromUrl(GameImageType.Screenshot, new Uri(screenshotUrl));
                }

                
                //todo add images
                return Tuple.Create(metadata, images);

            }

        }
    }
}
