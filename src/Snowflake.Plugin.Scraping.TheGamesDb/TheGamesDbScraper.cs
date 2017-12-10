using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Plugin.Scrapers.TheGamesDb.TheGamesDbApi;
using Snowflake.Scraping;
using Snowflake.Scraping.Attributes;
using static Snowflake.Utility.ScraperHelpers;
namespace Snowflake.Plugin.Scraping.TheGamesDb
{
    [RequiresRoot("platform")]
    [Plugin("TheGamesDBScraper")]
    public class TheGamesDbScraper : Scraper
    {
        private static IDictionary<string, string> map = new Dictionary<string, string>
        {
            { "ATARI_2600", "Atari 2600" },
            { "ATARI_5200", "Atari 5200" },
            { "ATARI_LYNX", "Atari Lynx" },
            { "NINTENDO_NES", "Nintendo Entertainment System (NES)" },
            { "NINTENDO_SNES", "Super Nintendo (SNES)" },
            { "NINTENDO_N64", "Nintendo 64" },
            { "NINTENDO_GCN", "Nintendo GameCube" },
            { "NINTENDO_WII", "Nintendo Wii" },
            { "NINTENDO_GB", "Nintendo Game Boy" },
            { "NINTENDO_GBC", "Nintendo Game Boy Color" },
            { "NINTENDO_GBA", "Nintendo Game Boy Advance" },
            { "NINTENDO_NDS", "Nintendo DS" },
            { "SONY_PSX", "Sony Playstation" },
            { "SONY_PS2", "Sony Playstation 2" },
            { "SONY_PSP", "Sony PSP" },
            { "SEGA_SMS", "Sega Master System" },
            { "SEGA_GEN", "Sega Genesis" },
            { "SEGA_MD", "Sega Mega Drive" },
            { "SEGA_CD", "Sega CD" },
            { "SEGA_32X", "Sega 32X" },
            { "SEGA_SAT", "Sega Saturn" },
            { "SEGA_DC", "Sega Dreamcast" },
            { "SEGA_GG", "Sega Game Gear" },
        };

        public TheGamesDbScraper()
            : base(typeof(TheGamesDbScraper), AttachTarget.Target, "search_title")
        {
        }

        public override async Task<IEnumerable<SeedTreeAwaitable>> ScrapeAsync(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            string platformId = rootSeeds["platform"].First().Value;
            string tgdbPlatform = TheGamesDbScraper.map[platformId];
            var results = await ApiGamesDb.GetGames(parent.Content.Value, tgdbPlatform)
                .ConfigureAwait(false);
            IList<SeedTreeAwaitable> resultTree = new List<SeedTreeAwaitable>();
            foreach (var result in results)
            {
                resultTree.Add(("result", result.Title, _(
                        ("title", result.Title),
                        ("platform", result.Platform)
                    )));
            }
            return resultTree;
        }
    }
}
