using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.IO;
using Snowflake.Extensibility;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Bunkai.Parsers;

namespace Snowflake.Plugin.Scraping.Filename
{
    [Plugin("Scraper-Filename")]
    public class FilenameScraper : Scraper
    {
        public FilenameScraper()
            // attach search targets to the root
            : base(typeof(FilenameScraper), AttachTarget.Root, "search_filename")
        {
            this.NoIntro = new NoIntroParser();
            this.Tosec = new TosecParser();
        }

        public NoIntroParser NoIntro { get; }
        public TosecParser Tosec { get; }

        public override async IAsyncEnumerable<SeedTree> ScrapeAsync(ISeed parent,
            ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds,
            ILookup<string, SeedContent> siblingSeeds, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            string filename = parent.Content.Value;
            string filenameWithoutExt = Path.GetFileNameWithoutExtension(filename);

            // prefer nointo first
            if (this.NoIntro.TryParse(filenameWithoutExt, out var nameInfo))
            {
                yield return ("search_title", nameInfo.NormalizedTitle);
                yield break;
            }

            if (this.Tosec.TryParse(filenameWithoutExt, out nameInfo))
            {
                yield return ("search_title", nameInfo.NormalizedTitle);
                yield break;
            }

            // if without ext succeeded, maybe try with ext?
            if (this.NoIntro.TryParse(filename, out nameInfo))
            {
                yield return ("search_title", nameInfo.NormalizedTitle);
                yield break;
            }

            if (this.Tosec.TryParse(filename, out nameInfo))
            {
                yield return ("search_title", nameInfo.NormalizedTitle);
                yield break;
            }
        }
    }
}
