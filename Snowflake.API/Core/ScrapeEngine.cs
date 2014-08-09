using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Information.Platform;
using Snowflake.Information.Game;
using Snowflake.Plugin;
using Snowflake.Constants;
using DuoVia.FuzzyStrings;
using Snowflake.Scraper;

namespace Snowflake.Core
{
    public class ScrapeEngine
    {
        private Platform ScrapePlatform { get; set; }
        private IScraper ScraperPlugin { get; set; }
        private IIdentifier IdentifierPlugin { get; set; }
        public ScrapeEngine(Platform scrapePlatform)
        {
            this.ScrapePlatform = scrapePlatform;
            this.ScraperPlugin = FrontendCore.LoadedCore.PluginManager.LoadedScrapers[ScrapePlatform.Defaults.Scraper];
            this.IdentifierPlugin = FrontendCore.LoadedCore.PluginManager.LoadedIdentifiers[ScrapePlatform.Defaults.Identifier];
        }

        public Game GetGameInfo(string fileName)
        {

            string gameName = this.IdentifierPlugin.IdentifyGame(fileName, this.ScrapePlatform.PlatformId);
            var results = this.ScraperPlugin.GetSearchResults(gameName, this.ScrapePlatform.PlatformId).OrderBy(result => result.GameTitle.LevenshteinDistance(gameName)).ToList();
            var resultdetails = this.ScraperPlugin.GetGameDetails(results[0].ID);
            var gameinfo = resultdetails.Item1;
            return new Game(
                this.ScrapePlatform.PlatformId,
                gameinfo[GameInfoFields.snowflake_game_title],
                resultdetails.Item2,
                gameinfo,
                ShortGuid.NewShortGuid(),
                fileName,
                new Dictionary<string, dynamic>()
            );
        }
    }
}
