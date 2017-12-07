using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Scraping
{
    public interface IScraper
    {
        AttachTarget Target { get; }
        bool IsGroup { get; }
        string GroupType { get; }
        string GroupValueType { get; }
        string ParentType { get; }
        string Name { get; }
        IEnumerable<string> RequiredChildSeeds { get; }
        IEnumerable<string> RequiredRootSeeds { get; }
        IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds);
    }
}
