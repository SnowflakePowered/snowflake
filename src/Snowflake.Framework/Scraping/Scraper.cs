using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Scraping.Attributes;
using Snowflake.Utility;
using System.Reflection;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Extensibility.Provisioning.Standalone;
using System.Threading.Tasks;

namespace Snowflake.Scraping
{
    public abstract class Scraper : ProvisionedPlugin, IScraper
    {
        public Scraper(Type pluginType,
                 AttachTarget target,
                 string targetType)
            : this(new StandalonePluginProvision(pluginType), target, targetType)
        {
        }

        public Scraper(IPluginProvision provision,
            AttachTarget target,
            string targetType)
            : base(provision)
        {
            this.AttachPoint = target;
            this.TargetType = targetType;
            this.Directives = this.GetType()
                .GetCustomAttributes<DirectiveAttribute>().ToList();
        }

        public AttachTarget AttachPoint { get; }

        public string TargetType { get; }

        public IEnumerable<IScraperDirective> Directives { get; }

        public abstract Task<IEnumerable<SeedTreeAwaitable>> ScrapeAsync(ISeed parent,
            ILookup<string, SeedContent> rootSeeds,
            ILookup<string, SeedContent> childSeeds,
            ILookup<string, SeedContent> siblingSeeds);
    }
}
