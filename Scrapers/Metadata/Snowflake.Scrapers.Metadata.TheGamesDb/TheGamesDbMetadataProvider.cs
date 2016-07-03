using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Scraper;
using Snowflake.Scraper.Providers;
using Snowflake.Scrapers.Metadata.TheGamesDb.TheGamesDbApi;

namespace Snowflake.Scrapers.Metadata.TheGamesDb
{
    public class TheGamesDbMetadataProvider : ScrapeProvider<IScrapedMetadataCollection>
    {
        private static IDictionary<string, string> map = new Dictionary<string, string>
        {
            {"ATARI_2600", "Atari 2600"},
            {"ATARI_5200", "Atari 5200"},
            {"ATARI_LYNX", "Atari Lynx"},
            {"NINTENDO_NES", "Nintendo Entertainment System (NES)"},
            {"NINTENDO_SNES", "Super Nintendo (SNES)"},
            {"NINTENDO_N64", "Nintendo 64"},
            {"NINTENDO_GCN", "Nintendo GameCube"},
            {"NINTENDO_WII", "Nintendo Wii"},
            {"NINTENDO_GB", "Nintendo Game Boy"},
            {"NINTENDO_GBC", "Nintendo Game Boy Color"},
            {"NINTENDO_GBA", "Nintendo Game Boy Advance"},
            {"NINTENDO_NDS", "Nintendo DS"},
            {"SONY_PSX", "Sony Playstation"},
            {"SONY_PS2", "Sony Playstation 2"},
            {"SONY_PSP", "Sony PSP"},
            {"SEGA_SMS", "Sega Master System"},
            {"SEGA_GEN", "Sega Genesis"},
            {"SEGA_MD", "Sega Mega Drive"},
            {"SEGA_CD", "Sega CD"},
            {"SEGA_32X", "Sega 32X"},
            {"SEGA_SAT", "Sega Saturn"},
            {"SEGA_DC", "Sega Dreamcast"},
            {"SEGA_GG", "Sega Game Gear"}
        };

        public override IEnumerable<IScrapedMetadataCollection> QueryAllResults(string searchQuery, string platformId)
        {
            string tgdbPlatform = TheGamesDbMetadataProvider.map[platformId];
            return (from r in ApiGamesDb.GetGames(searchQuery, tgdbPlatform)
                where r.Platform == tgdbPlatform
                let distance = LevenshteinDistance.Compute(TheGamesDbMetadataProvider.StripString(r.Title),
                    TheGamesDbMetadataProvider.StripString(searchQuery))
                orderby distance
                let gameMetadata = ApiGamesDb.GetGame(r.ID)
                let metadata = new ScrapedMetadataCollection("scraper_thegamesdb", (100 - distance) * 0.01)
                {
                    {GameMetadataKeys.Title, gameMetadata.Title},
                    {GameMetadataKeys.Description, gameMetadata.Overview},
                    {GameMetadataKeys.Publisher, gameMetadata.Publisher},
                    {GameMetadataKeys.ReleaseDate, gameMetadata.ReleaseDate},
                    {"scraper_thegamesdb_id", gameMetadata.ID.ToString()}
                }
                select metadata);
        }

        public override IScrapedMetadataCollection QueryBestMatch(string searchQuery, string platformId)
        {
            string tgdbPlatform = TheGamesDbMetadataProvider.map[platformId];
            var result = (from r in ApiGamesDb.GetGames(searchQuery, tgdbPlatform)
                          where r.Platform == tgdbPlatform
                          let distance = LevenshteinDistance.Compute(TheGamesDbMetadataProvider.StripString(r.Title), 
                          TheGamesDbMetadataProvider.StripString(searchQuery))
                          orderby distance
                          select new { r, distance }).FirstOrDefault();
            if (result == null) return null;
            var gameMetadata = ApiGamesDb.GetGame(result.r);
            IScrapedMetadataCollection metadata = new ScrapedMetadataCollection("scraper_thegamesdb", (100 - result.distance) * 0.01)
            {
                {GameMetadataKeys.Title, gameMetadata.Title},
                {GameMetadataKeys.Description, gameMetadata.Overview},
                {GameMetadataKeys.Publisher, gameMetadata.Publisher },
                {GameMetadataKeys.ReleaseDate, gameMetadata.ReleaseDate },
                {"scraper_thegamesdb_id", gameMetadata.ID.ToString() }
            };
            return metadata;
        }


        /// <summary>
        /// Removes spaces and punctuation from a string for Levenshtein comparison
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string StripString(string input)
        {
            return Regex.Replace(input, @"[^a-zA-Z0-9]", "");
        }

        [Provider]
        [RequiredMetadata(FileMetadataKeys.RomPlatform)]
        [RequiredMetadata(FileMetadataKeys.RomCanonicalTitle)]
        [ReturnMetadata(GameMetadataKeys.Title)]
        [ReturnMetadata(GameMetadataKeys.Description)]
        [ReturnMetadata(GameMetadataKeys.Platform)]
        [ReturnMetadata(GameMetadataKeys.ReleaseDate)]
        [ReturnMetadata("scraper_thegamesdb_id")]
        public IScrapedMetadataCollection WithPlatformAndSnowballTitle(IMetadataCollection collection)
        {
            return this.QueryBestMatch(collection[FileMetadataKeys.RomCanonicalTitle],
                collection[FileMetadataKeys.RomPlatform]);
        }

    }
}
