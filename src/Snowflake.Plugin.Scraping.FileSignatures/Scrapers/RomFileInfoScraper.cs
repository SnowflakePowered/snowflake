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
using static Snowflake.Scraping.Extensibility.SeedBuilder;

namespace Snowflake.Scrapers
{
    [Directive(AttachTarget.Root, Directive.Requires, "file")]
    [Directive(AttachTarget.Target, Directive.Requires, "mimetype")]
    [Plugin("RomFileInfoScraper")]
    public class RomFileInfoScraper : Scraper
    {
        IDictionary<string, IFileSignature> FileSignatures { get; }
        public RomFileInfoScraper()
          : base(typeof(RomFileInfoScraper), AttachTarget.Target, "file")
            {
            }

        public override async Task<IEnumerable<SeedTreeAwaitable>> ScrapeAsync(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds, ILookup<string, SeedContent> siblingSeeds)
        {
            string mimeType = childSeeds["mimetype"].First().Value;
            if (!File.Exists(parent.Content.Value) || !this.FileSignatures.ContainsKey(mimeType))
            {
                return _();
            }

            var signature = this.FileSignatures[mimeType];
            using (FileStream romStream = File.OpenRead(parent.Content.Value))
            {
                return _(
                  ("rom_serial", signature.GetSerial(romStream)),
                  ("rom_internal", signature.GetInternalName(romStream))
                );
            }
        }
    }
}
