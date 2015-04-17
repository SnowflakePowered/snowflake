using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;
using Snowflake.Game;
using Snowflake.Plugin;
using Snowflake.Constants;
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
        public ScrapeService(IPlatformInfo scrapePlatform, string scraperName)
        {
            this.ScrapePlatform = scrapePlatform;
            this.ScraperPlugin = CoreService.LoadedCore.PluginManager.LoadedScrapers[scraperName];
        }
        public ScrapeService(IPlatformInfo scrapePlatform) : this (scrapePlatform, CoreService.LoadedCore.PlatformPreferenceDatabase.GetPreferences(scrapePlatform).Scraper)
        {
        }

        public IList<IGameScrapeResult> GetGameResults(string fileName)
        {
            IDictionary<string, string> identifiedMetadata = CoreService.LoadedCore.PluginManager.LoadedIdentifiers.Values
                .Where(identifier => identifier.SupportedPlatforms.Contains(this.ScrapePlatform.PlatformID))
                .ToDictionary(identifier => identifier.PluginName,
                    identifier => identifier.IdentifyGame(fileName, this.ScrapePlatform.PlatformID));
            identifiedMetadata["md5"] = FileHash.GetMD5(fileName);
            identifiedMetadata["crc32"] = FileHash.GetCRC32(fileName);
            identifiedMetadata["sha1"] = FileHash.GetSHA1(fileName);
            identifiedMetadata["filename"] = fileName; 
            return this.ScraperPlugin.SortBestResults(identifiedMetadata, this.ScraperPlugin.GetSearchResults(identifiedMetadata, this.ScrapePlatform.PlatformID));
        }

        public IGameInfo GetGameInfo(IGameScrapeResult gameResult, string fileName)
        {
            return this.GetGameInfo(gameResult.ID);
        }

        public IGameInfo GetGameInfo(string id, string fileName)
        {
            var resultdetails = this.ScraperPlugin.GetGameDetails(id);
            var gameinfo = resultdetails.Item1;
            var gameUuid = FileHash.GetMD5(fileName);
            gameinfo.Add("snowflake_mediastorekey", ScrapeService.ValidateFilename(gameinfo[GameInfoFields.game_title]).Replace(' ', '_') + gameUuid);
            var gameResult = new GameInfo(
                this.ScrapePlatform.PlatformID,
                gameinfo[GameInfoFields.game_title],
                gameinfo,
                gameUuid,
                fileName
            );
            this.DownloadResults(resultdetails.Item2, gameResult.UUID);
            return gameResult;
        }
        public IGameImagesResult GetGameImageResults(string id)
        {
            var resultdetails = this.ScraperPlugin.GetGameDetails(id);
            return resultdetails.Item2;
        }
        private IGameMediaCache DownloadResults(IGameImagesResult imagesResult, string cacheKey)
        {
            IGameMediaCache mediaCache = new GameMediaCache(cacheKey);
            if (imagesResult.Boxarts.ContainsKey(ImagesInfoFields.img_boxart_back))
                mediaCache.SetBoxartBack(new Uri(imagesResult.Boxarts[ImagesInfoFields.img_boxart_back]));
            if (imagesResult.Boxarts.ContainsKey(ImagesInfoFields.img_boxart_front))
                mediaCache.SetBoxartFront(new Uri(imagesResult.Boxarts[ImagesInfoFields.img_boxart_front]));
            if (imagesResult.Boxarts.ContainsKey(ImagesInfoFields.img_boxart_full))
                mediaCache.SetBoxartFull(new Uri(imagesResult.Boxarts[ImagesInfoFields.img_boxart_full]));
            if(imagesResult.Fanarts.Count > 0)
                mediaCache.SetGameFanart(new Uri(imagesResult.Fanarts[0]));
            return mediaCache;
        }
        public IGameInfo GetGameInfo(string fileName)
        {
            var results = this.GetGameResults(fileName);
            return this.GetGameInfo(results[0], fileName);
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
