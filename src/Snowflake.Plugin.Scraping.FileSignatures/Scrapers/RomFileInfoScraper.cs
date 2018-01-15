using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Plugin.Scraping.FileSignatures.Composers;
using Snowflake.Romfile;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using static Snowflake.Scraping.Extensibility.SeedBuilder;

namespace Snowflake.Plugin.Scraping.FileSignatures.Scrapers
{
    [Directive(AttachTarget.Root, Directive.Requires, "file")]
    [Directive(AttachTarget.Target, Directive.Requires, "mimetype")]
    [Plugin("RomFileInfoScraper")]
    public class RomFileInfoScraper : Scraper
    {
        private FileSignatureCollection FileSignatures { get; }
        public RomFileInfoScraper(FileSignatureCollection fileSignatures)
          : base(typeof(RomFileInfoScraper), AttachTarget.Target, "file")
        {
            this.FileSignatures = fileSignatures;
        }

        public override async Task<IEnumerable<SeedTreeAwaitable>> ScrapeAsync(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds, ILookup<string, SeedContent> siblingSeeds)
        {
            string mimeType = childSeeds["mimetype"].First().Value;
            if (!File.Exists(parent.Content.Value) || !this.FileSignatures.Contains(mimeType))
            {
                return _();
            }

            var signature = this.FileSignatures[mimeType];
            using (FileStream romStream = File.OpenRead(parent.Content.Value))
            {
                string serial = signature.GetSerial(romStream);
                string internalName = signature.GetInternalName(romStream);

                IList<SeedTreeAwaitable> seedTrees = new List<SeedTreeAwaitable>();
                if (serial != null)
                {
                    seedTrees.Add(("rom_serial", serial));
                }

                if (internalName != null)
                {
                    seedTrees.Add(("rom_internal", internalName));
                }

                return seedTrees;
            }
        }
    }
}
