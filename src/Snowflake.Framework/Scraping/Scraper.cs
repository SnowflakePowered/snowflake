using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Scraping.Attributes;
using Snowflake.Utility;
using System.Reflection;

namespace Snowflake.Scraping
{
    public abstract class Scraper : IScraper
    {
        public Scraper()
        {
            var attachAttr = this.GetType().GetCustomAttribute<AttachAttribute>();
            if (attachAttr == null)
            {
                throw new MissingMemberException("Scraper must specify an attach target.");
            }

            this.Target = attachAttr.Target;
            var groupAttr = this.GetType().GetCustomAttribute<GroupAttribute>();
            this.IsGroup = groupAttr != null;
            this.GroupType = groupAttr?.GroupName;
            this.GroupValueType = groupAttr?.GroupOn;
            this.ParentType = this.GetType().GetCustomAttribute<ParentAttribute>().ParentType ?? SeedContent.RootSeedType;
            this.Name = this.GetType().Name; // todo: make this pluginName.
            this.RequiredChildSeeds = this.GetType().GetCustomAttributes<RequiresChildAttribute>().Select(p => p.Child).ToList();
            this.RequiredRootSeeds = this.GetType().GetCustomAttributes<RequiresRootAttribute>().Select(p => p.Child).ToList();

        }

        public AttachTarget Target { get; }

        public bool IsGroup { get; }

        public string GroupType { get; }

        public string GroupValueType { get; }

        public string ParentType { get; }

        public string Name { get; }

        public IEnumerable<string> RequiredChildSeeds { get; }

        public IEnumerable<string> RequiredRootSeeds { get; }

        public abstract IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds);
    }
}
