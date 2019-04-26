using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiantBomb.Api;
using Snowflake.Extensibility;
using Snowflake.Installation;
using Snowflake.Model.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using static Snowflake.Scraping.Extensibility.SeedBuilder;
using GameResult = GiantBomb.Api.Model.Game;
namespace Snowflake.Plugin.Scraping.GiantBomb
{
    [Directive(AttachTarget.Root, Directive.Requires, "platform")]
    [Plugin("ScraperGiantBomb")]
    public class GiantBombScraper : Scraper
    {
        private static IDictionary<PlatformId, int> platformMap = new Dictionary<PlatformId, int>
        {
         
            {"ARCADE_MAME" , 84},
            {"PANASONIC_3DO", 26},
            {"ATARI_2600",  40},
            {"ATARI_5200", 67},
            {"ATARI_7800", 70},
            {"ATARI_LYNX", 7},
            {"ATARI_JAGUAR", 28},
            {"NINTENDO_NES", 21},
            {"NINTENDO_SNES", 9},
            {"NINTENDO_N64", 43 },
            {"NINTENDO_N64DD", 101},
            {"NINTENDO_GCN", 23},
            {"NINTENDO_WII", 36}, // also 87 for wii shop
            {"NINTENDO_GB", 3},
            {"NINTENDO_GBC", 57},
            {"NINTENDO_GBA", 4},
            {"NINTENDO_NDS", 52}, // also 106 for DSiWare
            {"SONY_PSX", 22},
            {"SONY_PS2", 19},
            {"SONY_PSP", 18},
            {"SEGA_SMS", 8},
            {"SEGA_GEN", 6},
            {"SEGA_CD", 29},
            {"SEGA_32X", 31},
            {"SEGA_SAT", 42},
            {"SEGA_DC", 37},
            {"SEGA_GG", 5},
        };

        private static IDictionary<string, SortDirection> defaultSort = new Dictionary<string, SortDirection>
        {
            {"name", SortDirection.Ascending }
        };

        public GiantBombScraper()
            : base(typeof(GiantBombScraper), AttachTarget.Target, "search_title")
        {
            // this api key was made from a throwaway account.
            // will need to expose this as an option some day.
            this.GiantBombClient = new GiantBombRestClient("3d5cf51486f91a7e30318e03d06984dfb6e4930a"); 
        }

        
        public GiantBombRestClient GiantBombClient { get; }

        public override async IAsyncEnumerable<SeedTree> ScrapeAsync(ISeed parent,
            ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds,
            ILookup<string, SeedContent> siblingSeeds)
        {
            PlatformId platformId = rootSeeds["platform"].First().Value;
            bool platformSupported = platformMap.TryGetValue(platformId, out int giantBombPlatformId);
            if (!platformSupported) yield break;
            string searchQuery = parent.Content.Value;
            var results = await this.GiantBombClient.GetListResourceAsync<GameResult>
                ("games", 1, 100, null, null, new Dictionary<string, object> {
                    { "platforms" , giantBombPlatformId} }).ConfigureAwait(false);
            foreach (var result in results)
            {
               
                yield return ("result", result.Name, _(
                    ("title", result?.Name ?? ""),
                    ("description", result?.Deck ?? ""),
                    ("publisher", result?.Publishers != null ? String.Join(", ", result.Publishers.Select(p => p.Name)) : ""),
                    ("giantbomb_id", result?.Id.ToString() ?? "0"),
                    ("boxart", result?.Image?.SuperUrl ?? "")
                ));
            }
        }
    }
}
