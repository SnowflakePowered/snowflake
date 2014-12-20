using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Net;
using System.Xml.Linq;
using Snowflake.Constants;
using Snowflake.Scraper;

namespace Scraper.TheGamesDB
{
    public class TheGamesDB : BaseScraper
    {
        public TheGamesDB():base(Assembly.GetExecutingAssembly())
        {
        }

        private IList<GameScrapeResult> ParseSearchResults(Uri searchUri)
        {
            using (var client = new WebClient())
            {
                string xml = client.DownloadString(searchUri);
                XDocument xmlDoc = XDocument.Parse(xml);
                var games = (from game in xmlDoc.Descendants("Game") select game).ToList();
                var results = new List<GameScrapeResult>();
                foreach (var game in games)
                {
                    string id = game.Element("id").Value;
                    string title = game.Element("GameTitle").Value;
                    string platform = "UNKNOWN";
                    string xmlPlatformValue = game.Element("Platform").Value;
                    if (this.ScraperMap.Reverse.ContainsKey(xmlPlatformValue)) platform = this.ScraperMap.Reverse[xmlPlatformValue];

                    results.Add(new GameScrapeResult(id, platform, title));
                }
                return results;
            }
        }
        public override IList<GameScrapeResult> GetSearchResults(string searchQuery)
        {
            Console.WriteLine(this.CoreInstance.GetHashCode());
            var searchUri = new Uri(Uri.EscapeUriString("http://thegamesdb.net/api/GetGamesList.php?name=" + searchQuery));
            var results = ParseSearchResults(searchUri);
            return results;
        }
        public override IList<GameScrapeResult> GetSearchResults(string searchQuery, string platformId)
        {
            var searchUri = new Uri(Uri.EscapeUriString("http://thegamesdb.net/api/GetGamesList.php?name=" + searchQuery
                + "&platform=" + this.ScraperMap[platformId]));
            var results = ParseSearchResults(searchUri);
            return results;
        }
        public override Tuple<Dictionary<string, string>, GameImagesResult> GetGameDetails(string id)
        {
            var searchUri = new Uri(Uri.EscapeUriString("http://thegamesdb.net/api/GetGame.php?id=" + id));
            using (var client = new WebClient())
            {
                string xml = client.DownloadString(searchUri);
                XDocument xmlDoc = XDocument.Parse(xml);
                string baseImageUrl = xmlDoc.Descendants("baseImgUrl").First().Value;
                var metadata = new Dictionary<string, string>
                {
                    {GameInfoFields.game_description, xmlDoc.Descendants("Overview").First().Value},
                    {GameInfoFields.game_title, xmlDoc.Descendants("GameTitle").First().Value},
                    {GameInfoFields.game_releasedate, xmlDoc.Descendants("ReleaseDate").First().Value},
                    {GameInfoFields.game_publisher, xmlDoc.Descendants("Publisher").First().Value},
                    {GameInfoFields.game_developer, xmlDoc.Descendants("Developer").First().Value}
                };

                var images = new GameImagesResult();
                var boxartFront = baseImageUrl + (from boxart in xmlDoc.Descendants("boxart") where boxart.Attribute("side").Value == "front" select boxart).First().Value;
                images.AddFromUrl(GameImageType.Boxart_front, new Uri(boxartFront));

                var boxartBack = baseImageUrl + (from boxart in xmlDoc.Descendants("boxart") where boxart.Attribute("side").Value == "back" select boxart).First().Value;
                images.AddFromUrl(GameImageType.Boxart_back, new Uri(boxartBack));

                //Add fanarts
                var fanarts = (from fanart in xmlDoc.Descendants("fanart") select fanart).ToList();
                foreach (string fanartUrl in fanarts.Select(fanart => baseImageUrl + fanart.Element("original").Value))
                {
                    images.AddFromUrl(GameImageType.Fanart, new Uri(fanartUrl));
                }

                //Add screenshots
                var screenshots = (from screenshot in xmlDoc.Descendants("screenshot") select screenshot).ToList();
                foreach (string screenshotUrl in screenshots.Select(screenshot => baseImageUrl + screenshot.Element("original").Value))
                {
                    images.AddFromUrl(GameImageType.Screenshot, new Uri(screenshotUrl));
                }
                return Tuple.Create(metadata, images);
            }

        }
    }
}
