using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Service.Manager;
using Snowflake.Romfile;
using Snowflake.Scraper;
using System.IO;
using System.Security.Cryptography;
using Snowflake.Constants;
using Snowflake.Game;
using Snowflake.Platform;
using Snowflake.Utility;

namespace Snowflake.Service
{
    public class ScrapeEngine : IScrapeEngine
    {
      
        private readonly ICoreService coreService;
        
        public ScrapeEngine(ICoreService coreService)
        {
            this.coreService = coreService;
        }

        public IScrapableInfo GetScrapableInfo(string fileName, IPlatformInfo knownPlatform = null)
        {
            var fileSignatures = this.coreService.Get<IPluginManager>().Plugins<IFileSignature>();
            var vettedSignature = fileSignatures
                .Where(signature => signature.Value.FileExtensionMatches(fileName)).FirstOrDefault(signature => signature.Value.HeaderSignatureMatches(fileName)).Value;
            var platformFromFileExtension =
                this.coreService.Platforms.FirstOrDefault(
                    platform => platform.Value.FileExtensions.Contains(Path.GetExtension(fileName))).Value;
            var romPlatform =
                this.coreService.Platforms[
                    vettedSignature?.SupportedPlatform ??
                    knownPlatform?.PlatformID ?? 
                    platformFromFileExtension.PlatformID
                    ];
            return new ScrapableInfo(fileName, vettedSignature, romPlatform.PlatformID);
        }

        public IList<IGameScrapeResult> GetScrapeResults(IScrapableInfo information)
        {
            var scrapers =
               this.coreService.Get<IPluginManager>()
                   .Plugins<IScraper>()
                   .Where(scraper => scraper.Value.SupportedPlatforms.Contains(information.StonePlatformId)).Select(scraper => scraper.Value).OrderByDescending(scraper => scraper.ScraperAccuracy);
            return
                scrapers.SelectMany(scraper => scraper.GetSearchResults(information).OrderByDescending(result => result.Accuracy*scraper.ScraperAccuracy)).ToList();
        }
        public IList<IGameScrapeResult> GetScrapeResults(IScrapableInfo information, IScraper scraper)
        {
            return scraper.GetSearchResults(information).OrderByDescending(result => result.Accuracy * scraper.ScraperAccuracy).ToList();
        }

        public IGameInfo GetGameData(IScrapableInfo information, double acceptableAccuracy)
        {
            var scrapers =
                this.coreService.Get<IPluginManager>()
                    .Plugins<IScraper>()
                    .Where(scraper => scraper.Value.SupportedPlatforms.Contains(information.StonePlatformId))
                    .Select(scraper => scraper.Value)
                    .OrderByDescending(scraper => scraper.ScraperAccuracy);
            IDictionary<IGameScrapeResult, IScraper> candidateResults = new Dictionary<IGameScrapeResult, IScraper>();
            foreach (var scraper in scrapers)
            {
                var bestMatch =
                    scraper.GetSearchResults(information)
                        .OrderByDescending(result => result.Accuracy*scraper.ScraperAccuracy)?
                        .First();
                candidateResults.Add(bestMatch, scraper);
                if (bestMatch.Accuracy*scraper.ScraperAccuracy > acceptableAccuracy) break;
            }
            var bestResult =
                candidateResults.OrderByDescending(result => result.Key.Accuracy * result.Value.ScraperAccuracy).First();

            return this.GetGameData(information, bestResult.Key, bestResult.Value);
        }

        public IGameInfo GetGameData(IScrapableInfo information, IGameScrapeResult scrapeResult)
        {
            var scraper = this.coreService.Get<IPluginManager>().Plugin<IScraper>(scrapeResult.Scraper);
            return this.GetGameData(information, scrapeResult, scraper);
        }
        public IGameInfo GetGameData(IScrapableInfo information, IGameScrapeResult scrapeResult, IScraper scraper)
        {
            var gameInfo = scraper?.GetGameDetails(scrapeResult);
            var gameResult = new GameInfo(
                information.StonePlatformId,
                gameInfo.Item1[GameInfoFields.game_title],
                gameInfo.Item1,
                information.GetUUID(),
                information.OriginalFilePath);
            ScrapeEngine.GetMediaCache(gameInfo.Item2, gameResult);
            ScrapeEngine.GetScreenshotCache(gameInfo.Item2, gameResult);
            gameResult.Metadata.Add("rom_id", information.RomId);
            gameResult.Metadata.Add("rom_intername_name", information.RomInternalName);
            gameResult.Metadata.Add("rom_region", information.StructuredFilename.RegionCode);
            return gameResult;
        }
        public IGameInfo GetGameData(IScrapableInfo information)
        {
            return this.GetGameData(information, 1.0);
        }

        public IGameInfo GetGameData(IScrapableInfo information, IScraper scraper)
        {
            var bestResult = scraper.GetSearchResults(information)
                       .OrderByDescending(result => result.Accuracy * scraper.ScraperAccuracy)?
                       .First();
            var gameInfo = scraper.GetGameDetails(bestResult);
            var gameResult = new GameInfo(
                information.StonePlatformId,
                gameInfo.Item1[GameInfoFields.game_title],
                gameInfo.Item1,
                information.GetUUID(),
                information.OriginalFilePath);
            ScrapeEngine.GetMediaCache(gameInfo.Item2, gameResult);
            ScrapeEngine.GetScreenshotCache(gameInfo.Item2, gameResult);
            gameResult.Metadata.Add("rom_id", information.RomId);
            gameResult.Metadata.Add("rom_intername_name", information.RomInternalName);
            gameResult.Metadata.Add("rom_region", information.StructuredFilename.RegionCode);
            return gameResult;
        }

        private static IGameMediaCache GetMediaCache(IGameImagesResult imagesResult, IGameInfo gameInfo)
        {
            IGameMediaCache mediaCache = new GameMediaCache(gameInfo.UUID);
            if (imagesResult.Boxarts.ContainsKey(ImagesInfoFields.img_boxart_back))
                mediaCache.SetBoxartBack(new Uri(imagesResult.Boxarts[ImagesInfoFields.img_boxart_back]));
            if (imagesResult.Boxarts.ContainsKey(ImagesInfoFields.img_boxart_front))
                mediaCache.SetBoxartFront(new Uri(imagesResult.Boxarts[ImagesInfoFields.img_boxart_front]));
            if (imagesResult.Boxarts.ContainsKey(ImagesInfoFields.img_boxart_full))
                mediaCache.SetBoxartFull(new Uri(imagesResult.Boxarts[ImagesInfoFields.img_boxart_full]));
            if (imagesResult.Fanarts.Count > 0)
                mediaCache.SetGameFanart(new Uri(imagesResult.Fanarts[0]));
            return mediaCache;
        }

        private static IGameScreenshotCache GetScreenshotCache(IGameImagesResult imagesResult, IGameInfo gameInfo)
        {
            IGameScreenshotCache screenshotCache = new GameScreenshotCache(gameInfo.UUID);
            foreach (string screenshotUri in imagesResult.Screenshots)
            {
                screenshotCache.AddScreenshot(new Uri(screenshotUri));
            }
            return screenshotCache;
        }

       
    }
}
