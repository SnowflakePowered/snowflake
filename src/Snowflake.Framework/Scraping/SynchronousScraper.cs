using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Extensibility.Provisioning.Standalone;
using Snowflake.Scraping.Attributes;

namespace Snowflake.Scraping
{
    public abstract class SynchronousScraper : Scraper
    {
        public SynchronousScraper(Type pluginType,
                 AttachTarget target,
                 string targetType)
            : base(new StandalonePluginProvision(pluginType), target, targetType)
        {
        }

        public SynchronousScraper(IPluginProvision provision,
            AttachTarget target,
            string targetType)
            : base(provision, target, targetType)
        {
        }

        public abstract IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds,
            ILookup<string, SeedContent> childSeeds);

        public sealed override IEnumerable<Task<SeedContent>> ScrapeAsync(ISeed parent,
            ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            return this.Scrape(parent, rootSeeds, childSeeds).Select(p => Task.FromResult(p));
        }
    }
}
