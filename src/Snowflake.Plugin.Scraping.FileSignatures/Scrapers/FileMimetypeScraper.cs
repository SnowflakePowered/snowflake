using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeMapping;
using Snowflake.Extensibility;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using static Snowflake.Scraping.Extensibility.SeedBuilder;

namespace Snowflake.Plugin.Scraping.FileSignatures.Scrapers
{
    [Plugin("FileMimetypeScraper")]
    [Directive(AttachTarget.Root, Directive.Requires, "platform")]
    [Directive(AttachTarget.Target, Directive.Excludes, "mimetype")]
    public class FileMimetypeScraper : Scraper
    {
        private IStoneProvider StoneProvider { get; }

        public FileMimetypeScraper(IStoneProvider stoneProvider)
            : base(typeof(FileMimetypeScraper), AttachTarget.Target, "file")
        {
            this.StoneProvider = stoneProvider;
        }

        public override async Task<IEnumerable<SeedTreeAwaitable>>
            ScrapeAsync(ISeed parent, ILookup<string, SeedContent> rootSeeds,
            ILookup<string, SeedContent> childSeeds,
            ILookup<string, SeedContent> siblingSeeds)
        {
            string platformId = rootSeeds["platform"].First().Value;
            if (!this.StoneProvider.Platforms.TryGetValue(platformId, out var platform))
            {
                return _();
            }

            if (platform.FileTypes.TryGetValue(Path.GetExtension(parent.Content.Value), out string mimeType))
            {
                return _("mimetype", mimeType);
            }

            return _("mimetype", MimeUtility.GetMimeMapping(parent.Content.Value));
        }
    }
}
