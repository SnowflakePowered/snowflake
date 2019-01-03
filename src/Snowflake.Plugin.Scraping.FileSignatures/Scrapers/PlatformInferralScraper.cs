using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeMapping;
using Snowflake.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Plugin.Scraping.FileSignatures.Composers;
using Snowflake.Romfile;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using static Snowflake.Scraping.Extensibility.SeedBuilder;

namespace Snowflake.Plugin.Scraping.FileSignatures.Scrapers
{
    [Plugin("PlatformInferralScraper")]
    [Directive(AttachTarget.Root, Directive.Excludes, "platform")]
    [Directive(AttachTarget.Root, Directive.Requires, "file")]
    public class PlatformInferralScraper : Scraper
    {
        private IStoneProvider StoneProvider { get; }
        private FileSignatureCollection FileSignatures { get; }

        public PlatformInferralScraper(IStoneProvider stoneProvider, FileSignatureCollection fileSignatures)
            : base(typeof(PlatformInferralScraper), AttachTarget.Root, "file")
        {
            this.StoneProvider = stoneProvider;
            this.FileSignatures = fileSignatures;
        }

        private string GetMatchingMimetype(FileStream romStream)
        {
            foreach ((string mimetype, IFileSignature fileSignature) in this.FileSignatures)
            {
                try
                {
                    romStream.Position = 0;
                    if (fileSignature.HeaderSignatureMatches(romStream))
                    {
                        romStream.Position = 0;
                        return mimetype;
                    }
                }
                catch
                {
                    continue;
                }
            }

            return null;
        }

        private IPlatformInfo GetPlatformFromMimetype(string mimeType)
        {
            if (mimeType == null)
            {
                return null;
            }

            return this.StoneProvider.Platforms.Where(p => p.Value.FileTypes.Values
                    .Contains(mimeType, StringComparer.OrdinalIgnoreCase))
                .FirstOrDefault().Value;
        }

        private IPlatformInfo GetPlatformFromFileExtension(string fileExt)
        {
            return this.StoneProvider.Platforms.Where(p => p.Value.FileTypes.ContainsKey(Path.GetExtension(fileExt)))
                .FirstOrDefault().Value;
        }

        public override async Task<IEnumerable<SeedTreeAwaitable>>
            ScrapeAsync(ISeed parent, ILookup<string, SeedContent> rootSeeds,
                ILookup<string, SeedContent> childSeeds,
                ILookup<string, SeedContent> siblingSeeds)
        {
            if (!File.Exists(parent.Content.Value))
            {
                return _();
            }

            using (FileStream romStream = File.OpenRead(parent.Content.Value))
            {
                string mimeType = this.GetMatchingMimetype(romStream);
                var platform = this.GetPlatformFromMimetype(mimeType) ??
                               this.GetPlatformFromFileExtension(parent.Content.Value);
                if (platform != null)
                {
                    return _("platform", platform.PlatformId);
                }
            }

            return _();
        }
    }
}
