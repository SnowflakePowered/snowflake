using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Constants;
using Snowflake.Game;
using Snowflake.Identifier;
using Snowflake.Platform;
using Snowflake.Scraper;
using Snowflake.Service.Manager;
using Snowflake.Utility;

namespace Snowflake.Service
{
    public class ScrapeService : IScrapeService
    {
        private IPlatformInfo ScrapePlatform { get; }
        private IScraper ScraperPlugin { get; }
        public ICoreService CoreInstance { get; }
        public ScrapeService(IPlatformInfo scrapePlatform, string scraperName, ICoreService coreInstance)
        {
            this.ScrapePlatform = scrapePlatform;
            this.CoreInstance = coreInstance;
            this.ScraperPlugin = this.CoreInstance.Get<IPluginManager>().Plugins<IScraper>()[scraperName];
        }
        public ScrapeService(IPlatformInfo scrapePlatform, ICoreService coreInstance) : this (scrapePlatform, coreInstance.Get<IPlatformPreferenceDatabase>().GetPreferences(scrapePlatform).Scraper, coreInstance)
        {
        }

        public IList<IGameScrapeResult> GetGameResults(string fileName)
        {
            IList<IIdentifiedMetadata> identifiedMetadata = (from identifier in this.CoreInstance.Get<IPluginManager>().Plugins<IIdentifier>().Values
                                                             .Where(identifier => identifier.SupportedPlatforms.Contains(this.ScrapePlatform.PlatformID))
                                                             let value = identifier.IdentifyGame(fileName, this.ScrapePlatform.PlatformID)
                                                             select new IdentifiedMetadata(identifier.PluginName, identifier.IdentifiedValueType, value))
                                                             .Cast<IIdentifiedMetadata>().ToList();

            identifiedMetadata.Add(new IdentifiedMetadata("md5", IdentifiedValueTypes.FileHash, FileHash.GetMD5(fileName)));
            identifiedMetadata.Add(new IdentifiedMetadata("crc32", IdentifiedValueTypes.FileHash, FileHash.GetSHA1(fileName)));
            identifiedMetadata.Add(new IdentifiedMetadata("sha1", IdentifiedValueTypes.FileHash, FileHash.GetCRC32(fileName)));
            identifiedMetadata.Add(new IdentifiedMetadata("filename", IdentifiedValueTypes.FileName, FileHash.GetCRC32(fileName)));
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
            this.DownloadScreenshots(resultdetails.Item2, gameResult.UUID);
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
        private IGameScreenshotCache DownloadScreenshots(IGameImagesResult imagesResult, string cacheKey)
        {
            IGameScreenshotCache screenshotCache = new GameScreenshotCache(cacheKey);
            foreach (string screenshotUri in imagesResult.Screenshots)
            {
                screenshotCache.AddScreenshot(new Uri(screenshotUri));
            }
            return screenshotCache;
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
