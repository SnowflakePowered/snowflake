using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;
using Snowflake.Game;
using Snowflake.Plugin;
using Snowflake.Constants;
using DuoVia.FuzzyStrings;
using Snowflake.Scraper;
using System.IO;
using Snowflake.Identifier;
using Snowflake.Utility;

namespace Snowflake.Service
{
    public class ScrapeService : IScrapeService
    {
        private IPlatformInfo ScrapePlatform { get; set; }
        private IScraper ScraperPlugin { get; set; }
        public ScrapeService(IPlatformInfo scrapePlatform)
        {
            this.ScrapePlatform = scrapePlatform;
            this.ScraperPlugin = CoreService.LoadedCore.PluginManager.LoadedScrapers[ScrapePlatform.Defaults.Scraper];
        }

        public IGameInfo GetGameInfo(string fileName)
        {

            IDictionary<string, string> identifiedMetadata = CoreService.LoadedCore.PluginManager.LoadedIdentifiers.Values
                .Where(identifier => identifier.SupportedPlatforms.Contains(this.ScrapePlatform.PlatformId))
                .ToDictionary(identifier => identifier.PluginName,
                    identifier => identifier.IdentifyGame(fileName, this.ScrapePlatform.PlatformId));

            var results = this.ScraperPlugin.SortBestResults(identifiedMetadata, this.ScraperPlugin.GetSearchResults(identifiedMetadata, this.ScrapePlatform.PlatformId));
            var resultdetails = this.ScraperPlugin.GetGameDetails(results[0].ID);
            var gameinfo = resultdetails.Item1;
            var gameUuid = FileHash.GetMD5(fileName);
            return new GameInfo(
                this.ScrapePlatform.PlatformId,
                gameinfo[GameInfoFields.game_title],
                resultdetails.Item2.ToMediaStore("game."+ScrapeService.ValidateFilename(gameinfo[GameInfoFields.game_title]).Replace(' ','_')+gameUuid),
                gameinfo,
                gameUuid,
                fileName
            );
        }
        private static string ValidateFilename(string text, char? replacement = '_')
        {
            //from http://stackoverflow.com/a/25223884/1822679
            StringBuilder sb = new StringBuilder(text.Length);
            var invalids = Path.GetInvalidFileNameChars();
            bool changed = false;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (invalids.Contains(c))
                {
                    changed = true;
                    var repl = replacement ?? '\0';
                    if (repl != '\0')
                        sb.Append(repl);
                }
                else
                    sb.Append(c);
            }
            if (sb.Length == 0)
                return "_";
            return changed ? sb.ToString() : text;
        }
    }
}
