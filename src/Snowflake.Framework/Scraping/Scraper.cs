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
            var groupAttr = this.GetType().GetCustomAttribute<GroupAttribute>();
            this.IsGroup = groupAttr != null;
            this.GroupType = groupAttr?.GroupName;
            this.GroupValueType = groupAttr?.GroupOn;
            this.RequiredChildSeeds = this.GetType().GetCustomAttributes<RequiresChildAttribute>().Select(p => p.Child).ToList();
            this.RequiredRootSeeds = this.GetType().GetCustomAttributes<RequiresRootAttribute>().Select(p => p.Child).ToList();
        }

        public AttachTarget AttachPoint { get; }

        public bool IsGroup { get; }

        public string GroupType { get; }

        public string GroupValueType { get; }

        public string TargetType { get; }

        public IEnumerable<string> RequiredChildSeeds { get; }

        public IEnumerable<string> RequiredRootSeeds { get; }

        public abstract IEnumerable<Task<SeedContent>> ScrapeAsync(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds);
    }
}
