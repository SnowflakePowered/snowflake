using Snowflake.Extensibility;
using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Scraping
{
    public interface IScraper : IPlugin
    {
        AttachTarget AttachPoint { get; }
        string TargetType { get; }
        IEnumerable<string> RequiredChildSeeds { get; }
        IEnumerable<string> RequiredRootSeeds { get; }
        IEnumerable<SeedTreeAwaitable> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds);
    }
}
