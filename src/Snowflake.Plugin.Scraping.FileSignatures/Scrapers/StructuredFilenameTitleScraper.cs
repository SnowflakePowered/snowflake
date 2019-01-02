using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using static Snowflake.Scraping.Extensibility.SeedBuilder;

namespace Snowflake.Plugin.Scraping.FileSignatures.Scrapers
{
    [Plugin("StructuredFilenameTitleScraper")]
    [Directive(AttachTarget.Target, Directive.Requires, "mimetype")]
    public class StructuredFilenameTitleScraper : Scraper
    {
        HashSet<string> MimeTypes { get; }

        public StructuredFilenameTitleScraper(IStoneProvider stoneProvider)
            : base(typeof(StructuredFilenameTitleScraper), AttachTarget.Root, "file")
        {
            this.MimeTypes = stoneProvider
                .Platforms.SelectMany(p => p.Value.FileTypes.Select(f => f.Value)).ToHashSet();
        }

        public override async Task<IEnumerable<SeedTreeAwaitable>> ScrapeAsync(ISeed parent,
            ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds,
            ILookup<string, SeedContent> siblingSeeds)
        {
            if (!this.MimeTypes.Contains(childSeeds["mimetype"].First().Value))
            {
                return _();
            }

            var structured = new StructuredFilename(parent.Content.Value);
            return _("search_title", structured.Title);
        }
    }
}
