using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Extensibility.Provisioning.Standalone;

namespace Snowflake.Scraping.Extensibility
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
